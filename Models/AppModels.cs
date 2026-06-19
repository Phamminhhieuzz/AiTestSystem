using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiTestSystem.Models
{
    public class Level
    {
        [Key]
        public int LevelID { get; set; }
        [Required]
        public string LevelName { get; set; }
        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<TestResult> TestResults { get; set; }
    }

    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        public int LevelID { get; set; }
        [Required]
        public string Content { get; set; }

        [ForeignKey("LevelID")]
        public Level Level { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }

    public class Answer
    {
        [Key]
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        [Required]
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        [ForeignKey("QuestionID")]
        public Question Question { get; set; }
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TestResult> TestResults { get; set; }
    }

    public class TestResult
    {
        [Key]
        public int ResultID { get; set; }
        public int UserID { get; set; }
        public int TotalScore { get; set; }
        public int AssignedLevelID { get; set; }
        public DateTime TestDate { get; set; } = DateTime.Now;

        [ForeignKey("UserID")]
        public User User { get; set; }
        [ForeignKey("AssignedLevelID")]
        public Level Level { get; set; }
    }
}