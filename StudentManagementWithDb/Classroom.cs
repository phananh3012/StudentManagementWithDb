using Dapper;
using System;
using System.Data.SqlClient;

namespace StudentManagementWithDb
{
	public class Classroom
	{
		private readonly SqlConnection _connection;
		public Classroom(SqlConnection connection)
		{
			_connection = connection;
		}
		public int ClassroomId { get; set; }
		public int StudentId { get; set; }
		public int SubjectId { get; set; }
		public double FirstGrade { get; set; }
		public double SecondGrade { get; set; }
		public double FinalGrade { get; set; }
		public string Result { get; set; }

		private void CalGrade(int id)
		{
			string selectQuery = "SELECT SubjectId FROM Classroom WHERE ClassroomId = @ClassroomId";
			int subjectId = _connection.ExecuteScalar<int>(selectQuery, new { ClassroomId = id });
			string updateQuery = "";
			switch (subjectId)
			{
				case 1:
					updateQuery = "UPDATE Classroom SET FinalGrade = FirstGrade * 0.3 + SecondGrade * 0.7 where ClassroomId = @ClassroomId";
					break;
				case 2:
					updateQuery = "UPDATE Classroom SET FinalGrade = FirstGrade * 0.4 + SecondGrade * 0.6 where ClassroomId = @ClassroomId";
					break;
				case 3:
					updateQuery = "UPDATE Classroom SET FinalGrade = FirstGrade * 0.5 + SecondGrade * 0.5 where ClassroomId = @ClassroomId";
					break;
			}
			string updateResultQuery = "UPDATE Classroom SET Result = CASE WHEN FinalGrade > = 4 THEN 'Do' WHEN FinalGrade < 4 THEN 'Truot' ELSE null END";
			_connection.Execute(updateQuery, new { ClassroomId = id });
			_connection.Execute(updateResultQuery);
		}
		public void InsertGrade(int id)
		{
			Console.Write("Nhap diem qua trinh: ");
			double firstGrade = Convert.ToDouble(Console.ReadLine());
			Console.Write("Nhap diem thanh phan: ");
			double secondGrade = Convert.ToDouble(Console.ReadLine());
			string updateQuery = "UPDATE Classroom SET FirstGrade = @FirstGrade, SecondGrade = @SecondGrade WHERE ClassroomId = @ClassroomId";
			_connection.Execute(updateQuery, new { FirstGrade = firstGrade, SecondGrade = secondGrade, ClassroomId = id });
			CalGrade(id);
		}

	}
}
