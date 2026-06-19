using AiTestSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AiTestSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260619000000_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AiTestSystem.Models.Answer", b =>
            {
                b.Property<int>("AnswerID").ValueGeneratedOnAdd().HasColumnType("integer");
                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AnswerID"));
                b.Property<string>("Content").IsRequired().HasColumnType("text");
                b.Property<bool>("IsCorrect").HasColumnType("boolean");
                b.Property<int>("QuestionID").HasColumnType("integer");
                b.HasKey("AnswerID");
                b.HasIndex("QuestionID");
                b.ToTable("Answers");
            });

            modelBuilder.Entity("AiTestSystem.Models.Level", b =>
            {
                b.Property<int>("LevelID").ValueGeneratedOnAdd().HasColumnType("integer");
                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LevelID"));
                b.Property<string>("Description").IsRequired().HasColumnType("text");
                b.Property<string>("LevelName").IsRequired().HasColumnType("text");
                b.HasKey("LevelID");
                b.ToTable("Levels");
            });

            modelBuilder.Entity("AiTestSystem.Models.Question", b =>
            {
                b.Property<int>("QuestionID").ValueGeneratedOnAdd().HasColumnType("integer");
                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("QuestionID"));
                b.Property<string>("Content").IsRequired().HasColumnType("text");
                b.Property<int>("LevelID").HasColumnType("integer");
                b.HasKey("QuestionID");
                b.HasIndex("LevelID");
                b.ToTable("Questions");
            });

            modelBuilder.Entity("AiTestSystem.Models.TestResult", b =>
            {
                b.Property<int>("ResultID").ValueGeneratedOnAdd().HasColumnType("integer");
                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ResultID"));
                b.Property<int>("AssignedLevelID").HasColumnType("integer");
                b.Property<DateTime>("TestDate").HasColumnType("timestamp with time zone");
                b.Property<int>("TotalScore").HasColumnType("integer");
                b.Property<int>("UserID").HasColumnType("integer");
                b.HasKey("ResultID");
                b.HasIndex("AssignedLevelID");
                b.HasIndex("UserID");
                b.ToTable("TestResults");
            });

            modelBuilder.Entity("AiTestSystem.Models.User", b =>
            {
                b.Property<int>("UserID").ValueGeneratedOnAdd().HasColumnType("integer");
                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserID"));
                b.Property<DateTime>("CreatedAt").HasColumnType("timestamp with time zone");
                b.Property<string>("Department").IsRequired().HasColumnType("text");
                b.Property<string>("FullName").IsRequired().HasColumnType("text");
                b.HasKey("UserID");
                b.ToTable("Users");
            });

            modelBuilder.Entity("AiTestSystem.Models.Answer", b =>
            {
                b.HasOne("AiTestSystem.Models.Question", "Question")
                    .WithMany("Answers")
                    .HasForeignKey("QuestionID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                b.Navigation("Question");
            });

            modelBuilder.Entity("AiTestSystem.Models.Question", b =>
            {
                b.HasOne("AiTestSystem.Models.Level", "Level")
                    .WithMany("Questions")
                    .HasForeignKey("LevelID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                b.Navigation("Level");
            });

            modelBuilder.Entity("AiTestSystem.Models.TestResult", b =>
            {
                b.HasOne("AiTestSystem.Models.Level", "Level")
                    .WithMany("TestResults")
                    .HasForeignKey("AssignedLevelID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                b.HasOne("AiTestSystem.Models.User", "User")
                    .WithMany("TestResults")
                    .HasForeignKey("UserID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                b.Navigation("Level");
                b.Navigation("User");
            });

            modelBuilder.Entity("AiTestSystem.Models.Level", b =>
            {
                b.Navigation("Questions");
                b.Navigation("TestResults");
            });

            modelBuilder.Entity("AiTestSystem.Models.Question", b =>
            {
                b.Navigation("Answers");
            });

            modelBuilder.Entity("AiTestSystem.Models.User", b =>
            {
                b.Navigation("TestResults");
            });
#pragma warning restore 612, 618
        }
    }
}
