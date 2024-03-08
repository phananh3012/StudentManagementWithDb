using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace StudentManagementWithDb
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string connectionString = "Server= NAMNP-2020\\SQLEXPRESS; Database= StudentManagement; Trusted_Connection= True; TrustServerCertificate= True";
			SqlConnection connection = new SqlConnection(connectionString);
			Menu();

			void Menu()
			{
				bool flag = true;
				while (flag)
				{
					Console.WriteLine("Menu quan ly sinh vien");
					Console.WriteLine("1 - Xem danh sach sinh vien");
					Console.WriteLine("2 - Xem mon hoc sinh vien dang ky");
					Console.WriteLine("3 - Nhap diem sinh vien");
					Console.WriteLine("4 - Xem diem sinh vien");
					Console.WriteLine("5 - Thoat");

					string str = Console.ReadLine();
					switch (str)
					{
						case "1":
							Display();
							Console.ReadKey();
							Console.Clear();
							break;
						case "2":
							DisplaySubjects();
							Console.ReadKey();
							Console.Clear();
							break;
						case "3":
							Grading();
							Console.ReadKey();
							Console.Clear();
							break;
						case "4":
							DisplayGrade();
							Console.ReadKey();
							Console.Clear();
							break;
						case "5":
							flag = false;
							break;
						default:
							break;
					}
				}
			}

			void Display()
			{
				string selectQuery = "SELECT * FROM Students";
				List<Student> students = connection.Query<Student>(selectQuery).ToList();
				Console.WriteLine("Danh sach sinh vien");
				Console.WriteLine("MSV\tHo ten\t\t\tGioi tinh\tNgay sinh\tLop\tKhoa");
				foreach (Student student in students)
				{
					Console.WriteLine($"{student.StudentId}\t{student.StudentName}\t\t{(student.Gender == 1 ? "Nam" : "Nu")}\t\t{student.DOB.ToString("dd/mm/yyyy")}\t{student.Class}\t{student.Course}");
				}

			}

			void DisplaySubjects()
			{
				string selectQuery = @"SELECT ClassroomId, StudentName, SubjectName FROM Students, Subjects, Classroom 
										WHERE Students.StudentId = Classroom.StudentId and Subjects.SubjectId = Classroom.SubjectId";
				var classList = connection.Query(selectQuery).ToList();
				Console.WriteLine("Danh sach mon hoc");
				Console.WriteLine("STT\tHo ten\t\t\tMon hoc");
				foreach (var classMember in classList)
				{
					Console.WriteLine($"{classMember.ClassroomId}\t{classMember.StudentName}\t\t{classMember.SubjectName}");
				}


			}

			void Grading()
			{
				DisplaySubjects();
				Console.WriteLine("Nhap STT ban ghi can nhap diem: ");
				int id = Convert.ToInt32(Console.ReadLine());
				Classroom classroom = new Classroom(connection);
				classroom.InsertGrade(id);

			}

			void DisplayGrade()
			{
				string selectQuery = @"SELECT ClassroomId, StudentName, SubjectName, FirstGrade, SecondGrade, FinalGrade, Result FROM Classroom, Students, Subjects
										WHERE Classroom.StudentId = Students.StudentId AND Classroom.SubjectId = Subjects.SubjectId";
				var classList = connection.Query(selectQuery).ToList();

				Console.WriteLine("Bang diem sinh vien:");
				Console.WriteLine("STT\tHo ten\t\t\tMon hoc\tDiem QT\tDiem TP\tDiem tong ket\tKet qua");
				foreach (var classMember in classList)
				{
					Console.WriteLine($"{classMember.ClassroomId}\t{classMember.StudentName}\t\t{classMember.SubjectName}\t{classMember.FirstGrade}\t{classMember.SecondGrade}\t{classMember.FinalGrade}\t\t{classMember.Result}");
				}

			}
		}
	}
}
