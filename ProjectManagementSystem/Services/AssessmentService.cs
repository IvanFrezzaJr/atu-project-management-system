using ProjectManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ProjectManagementSystem.Domain.Services
{
    public class DatabaseService
    {
        private string _dbFile = "database.db";

        public void AddAssessment(Assessment assessment)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "INSERT INTO Assessment (Title, SubmissionDate, StudentId) VALUES (@Title, @SubmissionDate, @StudentId)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", assessment.Title);
                    command.Parameters.AddWithValue("@SubmissionDate", assessment.SubmissionDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@StudentId", assessment.StudentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Assessment> GetAssessmentsByStudent(int studentId)
        {
            var assessments = new List<Assessment>();

            using (var connection = new SQLiteConnection($"Data Source={_dbFile};Version=3;"))
            {
                connection.Open();
                string query = "SELECT Id, Title, SubmissionDate FROM Assessment WHERE StudentId = @StudentId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assessments.Add(new Assessment
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                SubmissionDate = DateTime.Parse(reader.GetString(2)),
                                StudentId = studentId
                            });
                        }
                    }
                }
            }

            return assessments;
        }
    }
}
