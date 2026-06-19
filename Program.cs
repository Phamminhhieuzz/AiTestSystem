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

// Auto-migrate + seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed data nếu chưa có
    if (!db.Levels.Any())
    {
        // --- 4 Levels ---
        var levels = new List<Level>
        {
            new Level { LevelName = "Nguoi Moi (Beginner)", Description = "Ban dang o muc co ban ve AI. Hay bat dau kham pha the gioi AI!" },
            new Level { LevelName = "Nguoi Dung (Intermediate)", Description = "Ban da co hieu biet co ban ve AI va co the su dung cac cong cu AI thong dung." },
            new Level { LevelName = "Nguoi Thao Tung (Advanced)", Description = "Ban co kien thuc sau ve AI va co the ung dung AI hieu qua vao cong viec." },
            new Level { LevelName = "Chuyen Gia AI (Expert)", Description = "Xuat sac! Ban co nang luc AI o trinh do chuyen gia, co the dan dat va huong dan nguoi khac." }
        };
        db.Levels.AddRange(levels);
        db.SaveChanges();

        // --- 40 Questions ---
        var questions = new List<Question>
        {
            // Chặng 1: Cơ Bản (LevelID=1, 10 câu)
            new Question { LevelID = 1, Content = "AI la viet tat cua cum tu nao?",
                Answers = new List<Answer> {
                    new Answer { Content = "Artificial Intelligence", IsCorrect = true },
                    new Answer { Content = "Automatic Interaction", IsCorrect = false },
                    new Answer { Content = "Advanced Internet", IsCorrect = false },
                    new Answer { Content = "Automated Information", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Cong cu AI nao sau day duoc biet den nhieu nhat de tro chuyen voi nguoi dung?",
                Answers = new List<Answer> {
                    new Answer { Content = "ChatGPT", IsCorrect = true },
                    new Answer { Content = "Photoshop", IsCorrect = false },
                    new Answer { Content = "Excel", IsCorrect = false },
                    new Answer { Content = "Google Maps", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Dieu gi xay ra khi ban 'prompt' mot AI chatbot?",
                Answers = new List<Answer> {
                    new Answer { Content = "Ban gui mot lenh hoac cau hoi cho AI de nhan ket qua", IsCorrect = true },
                    new Answer { Content = "Ban tai len mot tap tin cho AI", IsCorrect = false },
                    new Answer { Content = "Ban ket noi AI voi internet", IsCorrect = false },
                    new Answer { Content = "Ban khoi dong lai he thong AI", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Cong cu nao giup tao hinh anh tu van ban (text-to-image)?",
                Answers = new List<Answer> {
                    new Answer { Content = "Midjourney", IsCorrect = true },
                    new Answer { Content = "Grammarly", IsCorrect = false },
                    new Answer { Content = "Notion", IsCorrect = false },
                    new Answer { Content = "Slack", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Google cung cap cong cu AI chatbot nao canh tranh voi ChatGPT?",
                Answers = new List<Answer> {
                    new Answer { Content = "Gemini", IsCorrect = true },
                    new Answer { Content = "Bard 2.0", IsCorrect = false },
                    new Answer { Content = "Google Brain", IsCorrect = false },
                    new Answer { Content = "AlphaBot", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "LLM trong linh vuc AI la viet tat cua?",
                Answers = new List<Answer> {
                    new Answer { Content = "Large Language Model", IsCorrect = true },
                    new Answer { Content = "Linear Learning Machine", IsCorrect = false },
                    new Answer { Content = "Logical Language Module", IsCorrect = false },
                    new Answer { Content = "Large Learning Map", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Cong cu AI nao cua Microsoft duoc tich hop vao trinh duyet Edge?",
                Answers = new List<Answer> {
                    new Answer { Content = "Copilot", IsCorrect = true },
                    new Answer { Content = "Cortana Pro", IsCorrect = false },
                    new Answer { Content = "Azure Bot", IsCorrect = false },
                    new Answer { Content = "Bing Search", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "AI co the lam gi trong linh vuc viet noi dung?",
                Answers = new List<Answer> {
                    new Answer { Content = "Tao ban thao, tom tat, va chinh sua van ban", IsCorrect = true },
                    new Answer { Content = "In tai lieu tu dong", IsCorrect = false },
                    new Answer { Content = "Gui email cho khach hang", IsCorrect = false },
                    new Answer { Content = "Quan ly lich hen", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Dieu nao sau day mo ta dung nhat ve 'hallucination' cua AI?",
                Answers = new List<Answer> {
                    new Answer { Content = "AI tao ra thong tin sai su that nhung co ve tin cay", IsCorrect = true },
                    new Answer { Content = "AI bi loi va khong phan hoi", IsCorrect = false },
                    new Answer { Content = "AI hien thi hinh anh ao", IsCorrect = false },
                    new Answer { Content = "AI chay cham va tro nen qua tai", IsCorrect = false }
                }},
            new Question { LevelID = 1, Content = "Khi su dung AI cho cong viec, dieu quan trong nhat la?",
                Answers = new List<Answer> {
                    new Answer { Content = "Kiem tra va xac minh ket qua AI truoc khi su dung", IsCorrect = true },
                    new Answer { Content = "Su dung ket qua AI ma khong can xem lai", IsCorrect = false },
                    new Answer { Content = "Chi dung AI cho cong viec don gian", IsCorrect = false },
                    new Answer { Content = "Luon tra phi de dung ban AI tot nhat", IsCorrect = false }
                }},

            // Chặng 2: Ứng Dụng (LevelID=2, 10 câu)
            new Question { LevelID = 2, Content = "Ky thuat 'prompt engineering' la gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Nghe thuat viet cac cau lenh hieu qua de AI cho ket qua tot", IsCorrect = true },
                    new Answer { Content = "Lap trinh de tao ra AI moi", IsCorrect = false },
                    new Answer { Content = "Cai dat phan mem AI tren may tinh", IsCorrect = false },
                    new Answer { Content = "Ket noi nhieu AI voi nhau", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "Khi viet prompt cho AI, de dat ket qua tot nen:",
                Answers = new List<Answer> {
                    new Answer { Content = "Cu the, ro rang, cung cap du nguon canh", IsCorrect = true },
                    new Answer { Content = "Viet that ngan gon, it chu cang tot", IsCorrect = false },
                    new Answer { Content = "Dung nhieu tu viet tat de tiet kiem thoi gian", IsCorrect = false },
                    new Answer { Content = "Hoi nhieu cau mot luc de tiet kiem", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "Cong cu AI nao phu hop nhat de phan tich du lieu bang bieu va to Excel?",
                Answers = new List<Answer> {
                    new Answer { Content = "ChatGPT Advanced Data Analysis hoac Gemini Advanced", IsCorrect = true },
                    new Answer { Content = "Stable Diffusion", IsCorrect = false },
                    new Answer { Content = "Midjourney", IsCorrect = false },
                    new Answer { Content = "ElevenLabs", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "Chuc nang nao giup AI nho duoc cuoc tro chuyen dai va phan hoi nhat quan?",
                Answers = new List<Answer> {
                    new Answer { Content = "Context window - cua so ngu canh cua mo hinh", IsCorrect = true },
                    new Answer { Content = "Cache bo nho cung", IsCorrect = false },
                    new Answer { Content = "Ket noi internet toc do cao", IsCorrect = false },
                    new Answer { Content = "Tai khoan tra phi Premium", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "Ung dung nao duoi day su dung AI de tao nhac tu van ban?",
                Answers = new List<Answer> {
                    new Answer { Content = "Suno AI", IsCorrect = true },
                    new Answer { Content = "Canva", IsCorrect = false },
                    new Answer { Content = "Figma", IsCorrect = false },
                    new Answer { Content = "Notion AI", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "De bao ve thong tin bao mat khi dung AI cong ty, ban nen lam gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Khong nhap du lieu noi bo, mat khau, hoac thong tin khach hang vao AI cong cong", IsCorrect = true },
                    new Answer { Content = "Dung AI de luu tru tai khoan mat khau an toan hon", IsCorrect = false },
                    new Answer { Content = "Cho phep AI truy cap vao he thong noi bo de hoat dong hieu qua", IsCorrect = false },
                    new Answer { Content = "Chia se key API cho nhieu nguoi dung cung luc", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "AI co the ho tro viec dich thuat nhu the nao so voi cong cu truyen thong?",
                Answers = new List<Answer> {
                    new Answer { Content = "AI hieu nguon canh tot hon, dich tu nhien hon va co the dieu chinh van phong", IsCorrect = true },
                    new Answer { Content = "AI chi dich duoc ngon ngu phong bien", IsCorrect = false },
                    new Answer { Content = "AI dich cham hon nhung chinh xac hon", IsCorrect = false },
                    new Answer { Content = "AI khong ho tro dich tieng Viet", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "'Few-shot prompting' co nghia la gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Cung cap vai vi du mau cho AI de huong dan cach phan hoi", IsCorrect = true },
                    new Answer { Content = "Gui nhieu prompt mot luc de tang toc do", IsCorrect = false },
                    new Answer { Content = "Giam so luong tu trong prompt", IsCorrect = false },
                    new Answer { Content = "Su dung AI voi so lan gioi han trong ngay", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "Cong cu nao giup tao video tu van ban (text-to-video)?",
                Answers = new List<Answer> {
                    new Answer { Content = "Sora hoac Runway ML", IsCorrect = true },
                    new Answer { Content = "Adobe Photoshop", IsCorrect = false },
                    new Answer { Content = "Microsoft Word", IsCorrect = false },
                    new Answer { Content = "Google Docs", IsCorrect = false }
                }},
            new Question { LevelID = 2, Content = "RAG (Retrieval-Augmented Generation) giup AI lam gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Tim kiem thong tin tu nguon ben ngoai truoc khi tao phan hoi chinh xac hon", IsCorrect = true },
                    new Answer { Content = "Tang toc do xu ly cua AI", IsCorrect = false },
                    new Answer { Content = "Giam chi phi su dung AI", IsCorrect = false },
                    new Answer { Content = "Tu dong cap nhat mo hinh AI", IsCorrect = false }
                }},

            // Chặng 3: Nâng Cao (LevelID=3, 10 câu)
            new Question { LevelID = 3, Content = "Fine-tuning mot mo hinh AI co nghia la gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Huan luyen lai mo hinh tren bo du lieu chuyen biet de cai thien hieu suat cho nhu vu cu the", IsCorrect = true },
                    new Answer { Content = "Cap nhat phan mem AI len phien ban moi", IsCorrect = false },
                    new Answer { Content = "Chinh sua giao dien cua ung dung AI", IsCorrect = false },
                    new Answer { Content = "Giam kich thuoc mo hinh AI de chay tren thiet bi yeu", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Token trong cac mo hinh ngon ngu lon la gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Don vi xu ly van ban (co the la tu, phan tu tu, hoac ky tu)", IsCorrect = true },
                    new Answer { Content = "Don vi tien te de thanh toan dich vu AI", IsCorrect = false },
                    new Answer { Content = "Chung chi xac thuc truy cap API", IsCorrect = false },
                    new Answer { Content = "Phan tu trong mang no-ron nhan tao", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Kien truc Transformer trong AI duoc gioi thieu lan dau qua bai bao nao?",
                Answers = new List<Answer> {
                    new Answer { Content = "Attention Is All You Need (Google, 2017)", IsCorrect = true },
                    new Answer { Content = "ImageNet Classification (2012)", IsCorrect = false },
                    new Answer { Content = "Deep Residual Learning (2015)", IsCorrect = false },
                    new Answer { Content = "Generative Adversarial Nets (2014)", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Embeddings trong AI la gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Bieu dien so hoc cua van ban/hinh anh trong khong gian vector nhieu chieu", IsCorrect = true },
                    new Answer { Content = "Qua trinh nhung code vao trong anh", IsCorrect = false },
                    new Answer { Content = "Dinh dang file dac biet cho du lieu AI", IsCorrect = false },
                    new Answer { Content = "Cach ma hoa du lieu truoc khi gui cho AI", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "AI Agent khac AI Chatbot o diem nao?",
                Answers = new List<Answer> {
                    new Answer { Content = "Agent co the tu dong thuc hien chuoi hanh dong va su dung cong cu de hoan thanh muc tieu", IsCorrect = true },
                    new Answer { Content = "Agent chay nhanh hon Chatbot", IsCorrect = false },
                    new Answer { Content = "Agent re hon va tiet kiem tai nguyen hon", IsCorrect = false },
                    new Answer { Content = "Agent chi hoat dong voi van ban, khong xu ly hinh anh", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Temperature trong cac mo hinh AI dieu chinh dieu gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Muc do ngau nhien va sang tao trong cac phan hoi (cao = sang tao hon, thap = xac dinh hon)", IsCorrect = true },
                    new Answer { Content = "Nhiet do may chu de tranh qua tai", IsCorrect = false },
                    new Answer { Content = "Toc do tao phan hoi", IsCorrect = false },
                    new Answer { Content = "Kich thuoc cua so ngu canh", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Ky thuat 'Chain-of-Thought prompting' hoat dong nhu the nao?",
                Answers = new List<Answer> {
                    new Answer { Content = "Yeu cau AI giai thich tung buoc tu duy truoc khi dua ra dap an cuoi cung", IsCorrect = true },
                    new Answer { Content = "Ket noi nhieu prompt voi nhau bang chuoi", IsCorrect = false },
                    new Answer { Content = "Su dung AI de phan tich chuoi cung ung", IsCorrect = false },
                    new Answer { Content = "Gui prompt theo tung dot nho thay vi mot lan", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Vector database dung de lam gi trong he thong AI?",
                Answers = new List<Answer> {
                    new Answer { Content = "Luu tru va tim kiem embeddings de ho tro cac ung dung RAG va semantic search", IsCorrect = true },
                    new Answer { Content = "Luu tru code cua mo hinh AI", IsCorrect = false },
                    new Answer { Content = "Quan ly tai khoan nguoi dung AI", IsCorrect = false },
                    new Answer { Content = "Xu ly hinh anh dang vector SVG", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "Multi-modal AI co kha nang gi dac biet?",
                Answers = new List<Answer> {
                    new Answer { Content = "Xu ly va ket hop nhieu loai du lieu: van ban, hinh anh, am thanh, video", IsCorrect = true },
                    new Answer { Content = "Chay tren nhieu he dieu hanh cung luc", IsCorrect = false },
                    new Answer { Content = "Ho tro nhieu ngon ngu lap trinh", IsCorrect = false },
                    new Answer { Content = "Ket noi nhieu may chu cung luc", IsCorrect = false }
                }},
            new Question { LevelID = 3, Content = "RLHF (Reinforcement Learning from Human Feedback) duoc dung de lam gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Huan luyen AI theo phan hoi cua con nguoi de phan hoi phu hop va an toan hon", IsCorrect = true },
                    new Answer { Content = "Thung thuong nhan vien su dung AI tot nhat", IsCorrect = false },
                    new Answer { Content = "Thu thap phan hoi khach hang tu dong", IsCorrect = false },
                    new Answer { Content = "Hoc may tang cuong tu moi truong ao", IsCorrect = false }
                }},

            // Chặng 4: Chuyên Gia (LevelID=4, 10 câu)
            new Question { LevelID = 4, Content = "MoE (Mixture of Experts) trong kien truc AI hoat dong theo nguyen tac nao?",
                Answers = new List<Answer> {
                    new Answer { Content = "Chi kich hoat mot so luong nho 'chuyen gia' (sub-networks) cho moi token, giam chi phi tinh toan", IsCorrect = true },
                    new Answer { Content = "Ket hop nhieu AI khac nhau de bau chon ket qua tot nhat", IsCorrect = false },
                    new Answer { Content = "Phan cong cong viec cho nhom nhan su chuyen mon", IsCorrect = false },
                    new Answer { Content = "Su dung nhieu GPU de xu ly song song toan bo mo hinh", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "GAN (Generative Adversarial Network) bao gom hai thanh phan chinh la gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Generator (tao du lieu gia) va Discriminator (phan biet that/gia)", IsCorrect = true },
                    new Answer { Content = "Encoder va Decoder", IsCorrect = false },
                    new Answer { Content = "Trainer va Validator", IsCorrect = false },
                    new Answer { Content = "Producer va Consumer", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Khi xay dung he thong AI Agent phuc tap, 'tool calling' (function calling) co vai tro gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Cho phep LLM yeu cau thuc thi cac ham ben ngoai (API, DB, code) de lay thong tin hoac thuc hien hanh dong", IsCorrect = true },
                    new Answer { Content = "Goi dien thoai tu dong cho khach hang", IsCorrect = false },
                    new Answer { Content = "Chay cac thu vien Python ben trong AI", IsCorrect = false },
                    new Answer { Content = "Ket noi nhieu mo hinh AI voi nhau qua API", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Catastrophic forgetting trong AI la hien tuong gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Mo hinh quen kien thuc cu khi duoc fine-tune tren du lieu moi", IsCorrect = true },
                    new Answer { Content = "AI that bai hoan toan sau mot thoi gian khong su dung", IsCorrect = false },
                    new Answer { Content = "Du lieu huan luyen bi mat do su co ky thuat", IsCorrect = false },
                    new Answer { Content = "Model bi qua tai va crash khi xu ly qua nhieu request", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Constitutional AI (CAI) cua Anthropic co y nghia gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Phuong phap train AI theo mot tap cac nguyen tac dao duc, de AI tu danh gia va sua chinh phan hoi cua minh", IsCorrect = true },
                    new Answer { Content = "AI duoc bao ve boi luat phap cua chinh phu", IsCorrect = false },
                    new Answer { Content = "Kien truc AI duoc xay dung theo hiep phap quoc gia", IsCorrect = false },
                    new Answer { Content = "He thong AI chi hoat dong trong phap quy cho phep", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Trong huan luyen LLM, 'PEFT' (Parameter-Efficient Fine-Tuning) nhu LoRA giup gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Fine-tune mo hinh lon voi so luong tham so duoc cap nhat rat nho, tiet kiem VRAM va thoi gian", IsCorrect = true },
                    new Answer { Content = "Tang kich thuoc mo hinh de cai thien hieu suat", IsCorrect = false },
                    new Answer { Content = "Tu dong toi uu hoa sieu tham so (hyperparameters)", IsCorrect = false },
                    new Answer { Content = "Chia mo hinh lon thanh nhieu mo hinh nho hon", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Context window 1 trieu token cua Gemini 1.5 co y nghia thuc tien gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Co the phan tich toan bo code base, sach, hoac lich su cuoc tro chuyen dai ma khong bi cat ngan", IsCorrect = true },
                    new Answer { Content = "Co the xu ly 1 trieu nguoi dung cung luc", IsCorrect = false },
                    new Answer { Content = "Mo hinh co the hoc them 1 trieu khai niem moi", IsCorrect = false },
                    new Answer { Content = "Gio han su dung API tang len 1 trieu request/ngay", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "AI Alignment la linh vuc nghien cuu gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Dam bao AI hanh dong phu hop voi gia tri, muc tieu va y dinh cua con nguoi", IsCorrect = true },
                    new Answer { Content = "Can chinh du lieu huan luyen cho dung dinh dang", IsCorrect = false },
                    new Answer { Content = "Dong bo hoa nhieu mo hinh AI voi nhau", IsCorrect = false },
                    new Answer { Content = "Cai thien toc do suy luan cua mo hinh AI", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Speculative decoding trong suy luan LLM giup gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Dung mo hinh nho de du doan nhieu token truoc, mo hinh lon kiem tra, tang toc do suy luan dang ke", IsCorrect = true },
                    new Answer { Content = "Du doan gia co phieu dua tren du lieu thi truong", IsCorrect = false },
                    new Answer { Content = "Giam kich thuoc mo hinh de deploy de hon", IsCorrect = false },
                    new Answer { Content = "Tu dong mo rong tai nguyen may chu khi co nhieu request", IsCorrect = false }
                }},
            new Question { LevelID = 4, Content = "Trong danh gia LLM, 'benchmark' nhu MMLU, HumanEval, HELM duoc dung de lam gi?",
                Answers = new List<Answer> {
                    new Answer { Content = "Do luong va so sanh kha nang cua cac mo hinh tren cac nhu vu chuan hoa da dang", IsCorrect = true },
                    new Answer { Content = "Do toc do xu ly request cua he thong AI", IsCorrect = false },
                    new Answer { Content = "Kiem tra bao mat va lo hong cua mo hinh", IsCorrect = false },
                    new Answer { Content = "Theo doi muc tieu kinh doanh cua cong ty AI", IsCorrect = false }
                }},
        };

        db.Questions.AddRange(questions);
        db.SaveChanges();
    }
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