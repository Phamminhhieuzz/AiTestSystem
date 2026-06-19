using AiTestSystem.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- KẾT NỐI DATABASE (Railway PostgreSQL) ---
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Chuyển đổi DATABASE_URL (postgres:// hoặc postgresql://) sang Npgsql format
if (connectionString != null && (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();

var app = builder.Build();

// Auto-migrate database khi khởi động
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles(); 
app.UseStaticFiles();  
app.UseHttpsRedirection();

// API 1: Lấy danh sách câu hỏi đưa lên Giao diện
app.MapGet("/api/questions", async (AppDbContext db) =>
{
    var questions = await db.Questions
        .Select(q => new {
            q.QuestionID,
            q.Content,
            Answers = q.Answers.Select(a => new { a.AnswerID, a.Content }) 
        })
        .ToListAsync();
        
    return Results.Ok(questions);
});

// API 2: Nhận bài làm, chấm điểm và trả về danh sách review Đúng/Sai
app.MapPost("/api/submit", async (SubmitRequest req, AppDbContext db) =>
{
    int totalScore = 0;
    var reviewList = new List<QuestionReviewDto>();

    // Lấy toàn bộ câu hỏi kèm đáp án từ DB để đối chiếu bảo mật trong bộ nhớ
    var questionsInDb = await db.Questions.Include(q => q.Answers).ToListAsync();

    foreach (var q in questionsInDb)
    {
        // Tìm xem người dùng đã chọn đáp án nào cho câu hỏi này
        var userAnswerId = req.SelectedAnswerIds
            .FirstOrDefault(id => q.Answers.Any(a => a.AnswerID == id));

        // Tìm đáp án đúng của câu hỏi này trong DB
        var correctAnswer = q.Answers.FirstOrDefault(a => a.IsCorrect);
        
        bool isCorrect = userAnswerId != 0 && userAnswerId == correctAnswer?.AnswerID;
        if (isCorrect) totalScore++;

        // Thêm vào danh sách review trả về cho Client
        reviewList.Add(new QuestionReviewDto(
            q.QuestionID,
            userAnswerId, // Bằng 0 nếu bỏ qua không khoanh
            correctAnswer?.AnswerID ?? 0
        ));
    }

    // Logic phân loại Level
    int assignedLevelId = 1; 
    if (totalScore >= 31) assignedLevelId = 4;      
    else if (totalScore >= 21) assignedLevelId = 3; 
    else if (totalScore >= 11) assignedLevelId = 2; 
    else assignedLevelId = 1;                       

    var level = await db.Levels.FindAsync(assignedLevelId);

    // Lưu thông tin người làm bài và kết quả điểm số vào DB
    var user = new User { FullName = req.FullName, Department = req.Department, CreatedAt = DateTime.UtcNow };
    db.Users.Add(user);
    await db.SaveChangesAsync();

    var result = new TestResult { UserID = user.UserID, TotalScore = totalScore, AssignedLevelID = assignedLevelId, TestDate = DateTime.UtcNow };
    db.TestResults.Add(result);
    await db.SaveChangesAsync();

    // Trả kết quả kèm mảng reviews
    return Results.Ok(new {
        score = totalScore,
        levelName = level?.LevelName ?? "Chưa phân loại",
        description = level?.Description ?? "",
        reviews = reviewList
    });
});

app.Run();

// Định nghĩa cấu trúc dữ liệu
public record SubmitRequest(string FullName, string Department, List<int> SelectedAnswerIds);
public record QuestionReviewDto(int QuestionId, int UserAnswerId, int CorrectAnswerId);