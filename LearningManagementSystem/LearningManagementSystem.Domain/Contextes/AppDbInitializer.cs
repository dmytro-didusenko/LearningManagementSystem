using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Contextes
{
    public static class ModelBuilderExtensions
    {
        private static List<User> _users = GetUsers();
        private static List<Student> _students = GetStudents();
        private static List<Group> _groups = GetGroups();
        private static List<Course> _courses = GetCourses();
        private static List<Subject> _subjects = GetSubjects();
        private static List<Topic> _topics = GetTopics();
        private static List<HomeTask> _homeTasks = GetHomeTasks();
        private static List<TaskAnswer> _taskAnswers = GetTaskAnswers();
        private static List<Grade> _grades = GetGrades();
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(_users);
            modelBuilder.Entity<Student>().HasData(_students);
            modelBuilder.Entity<Group>().HasData(_groups);
            modelBuilder.Entity<Course>().HasData(_courses);
            modelBuilder.Entity<Subject>().HasData(_subjects);
            modelBuilder.Entity<Topic>().HasData(_topics);
            modelBuilder.Entity<HomeTask>().HasData(_homeTasks);
            modelBuilder.Entity<TaskAnswer>().HasData(_taskAnswers);
            modelBuilder.Entity<Grade>().HasData(_grades);
        }

        public static List<User> GetUsers()
        {
            return new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Harry",
                    LastName = "Potter",
                    Birthday = new DateTime(2001, 4, 16),
                    Email = "harry@gmail.com",
                    About = "Hello, I am Harry",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "harryPotter"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Linus",
                    LastName = "Torvalds",
                    Birthday = new DateTime(1969, 12, 28),
                    Email = "torvalds@gmail.com",
                    About = "I know Linux",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "linusTorvalds"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bill",
                    LastName = "Gates",
                    Birthday = new DateTime(1955, 10, 28),
                    Email = "billGates@gmail.com",
                    About = "Hello, I am Microsoft creator",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "gates",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Anna",
                    LastName = "Hotsuliak",
                    Birthday = new DateTime(2005, 4, 3),
                    Email = "annaHotsuliak@gmail.com",
                    About = "Some info about me",
                    Gender = Gender.Female,
                    IsActive = true,
                    UserName = "anna",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Denis",
                    LastName = "Brown",
                    Birthday = new DateTime(2005, 5, 25),
                    Email = "brown@gmail.com",
                    About = string.Empty,
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "deniBrown",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jeff",
                    LastName = "Bezos",
                    Birthday = new DateTime(1964, 1, 12),
                    Email = "bezos@amazon.com",
                    About = "I am Amazon creator",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "JeffBezos",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Mark",
                    LastName = "Zuckerberg",
                    Birthday = new DateTime(1984, 5, 14),
                    Email = "markFacebook@facebook.com",
                    About = "Facebook is a bullshit!",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "markZucker",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Sergey",
                    LastName = "Brin",
                    Birthday = new DateTime(1973, 8, 21),
                    Email = "mrBrin@gmail.com",
                    About = "I like to learn GO",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "serhiiBrin",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Viktoria",
                    LastName = "Holmes",
                    Birthday = new DateTime(2002, 2, 13),
                    Email = "holmesViki@gmail.com",
                    About = "I wanna be a frontend-dev",
                    Gender = Gender.Female,
                    IsActive = true,
                    UserName = "holmesVik",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jeremy",
                    LastName = "Clarkson",
                    Birthday = new DateTime(1660, 4, 11),
                    Email = "clarkson@gmail.com",
                    About = "I wanna become a backend-dev",
                    Gender = Gender.Male,
                    IsActive = true,
                    UserName = "clarksonJ",
                }
            };
        }

        public static List<Student> GetStudents()
        {
            return new List<Student>()
            {
                new()
                {
                    Id = _users[0].Id,
                    ContractNumber = "CONTRACT-85972",
                    GroupId = _groups[0].Id,
                },
                new()
                {
                    Id = _users[1].Id,
                    ContractNumber = "CONTRACT-89634",
                    GroupId = _groups[0].Id,
                },
                new()
                {
                    Id = _users[2].Id,
                    ContractNumber = "CONTRACT-14753",
                    GroupId = _groups[0].Id,
                },
                new()
                {
                    Id = _users[3].Id,
                    ContractNumber = "CONTRACT-15873",
                    GroupId = _groups[0].Id,
                },
                new()
                {
                    Id = _users[4].Id,
                    ContractNumber = "CONTRACT-02155",
                    GroupId = _groups[0].Id,
                },
                new()
                {
                    Id = _users[5].Id,
                    ContractNumber = "CONTRACT-10105",
                    GroupId = _groups[1].Id,
                },
                new()
                {
                    Id = _users[6].Id,
                    ContractNumber = "CONTRACT-87415",
                    GroupId = _groups[1].Id,
                },
                new()
                {
                    Id = _users[7].Id,
                    ContractNumber = "CONTRACT-14756",
                    GroupId = _groups[1].Id,
                },
                new()
                {
                    Id = _users[8].Id,
                    ContractNumber = "CONTRACT-30257",
                    GroupId = _groups[1].Id,
                },
                new()
                {
                    Id = _users[9].Id,
                    ContractNumber = "CONTRACT-99999",
                    GroupId = _groups[1].Id,
                }
            };
        }

        public static List<Group> GetGroups()
        {
            return new List<Group>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Summer .NET group",
                    StartEducation = new DateTime(2022, 5, 15),
                    EndEducation = new DateTime(2022, 9, 1),
                    CourseId = _courses[0].Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Summer Front-End group",
                    StartEducation = new DateTime(2022, 5, 20),
                    EndEducation = new DateTime(2022, 9, 8),
                    CourseId = _courses[1].Id
                }
            };
        }

        public static List<Course> GetCourses()
        {
            return new List<Course>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = ".NET course",
                    Description = "This is course about .NET and back-end",
                    StartedAt = new DateTime(2022, 5, 12),
                    Subjects = new List<Subject>(){_subjects[0], _subjects[1], _subjects[2], _subjects[4], _subjects[5]}
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Front-End course",
                    Description = "This is course about front-end",
                    StartedAt = new DateTime(2022, 5, 12),
                    Subjects = new List<Subject>(){_subjects[0], _subjects[1], _subjects[3], _subjects[4]}
                },
            };
        }

        public static List<Subject> GetSubjects()
        {
            return new List<Subject>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Computer Science Basics",
                    Courses = _courses
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Mathematics",
                    Courses= _courses
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = ".NET Basics",
                    Courses = new List<Course>(){_courses[0]}
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name =  "JS Fundamentals",
                    Courses = new List<Course>(){_courses[1]}
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Databases",
                    Courses = _courses
                },
                new()
                {
                    Id = new Guid(),
                    Name = "ASP.NET Core",
                    Courses = new List<Course>(){_courses[0]}
                }
            };
        }

        public static List<Topic> GetTopics()
        {
            return new List<Topic>()
            {
                //subject 0
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 1. CS Basics",
                    Content = "Some content",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[0].Id,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 2. C Language",
                    Content = "Some content about topic 2",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[0].Id,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 3. Python",
                    Content = "Some content about Python",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[0].Id,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 4. Math in CS",
                    Content = "Some content",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[0].Id,
                },
                //subject 2
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 1. .NET",
                    Content = "Some content about .NET",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[2].Id,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 2. .NET Core",
                    Content = "Some content about .NET",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[2].Id,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 3. ASP.NET Core",
                    Content = "Some content about ASP NET Core",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[2].Id,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Topic 4. Redis",
                    Content = "Some content about Redis",
                    DateOfCreation = DateTime.Now,
                    SubjectId = _subjects[2].Id,
                }
            };
        }

        public static List<HomeTask> GetHomeTasks()
        {
            return new List<HomeTask>()
            {
                new()
                {
                    TopicId = _topics[0].Id,
                    Name = "HW-01 CS",
                    DatePlannedStart = DateTime.Now,
                    DateOfExpiration = new DateTime(2022, 6, 28),
                    Description = "Simple hw"
                },
                new()
                {
                    TopicId = _topics[1].Id,
                    Name = "HW-02 C Lang",
                    DatePlannedStart = DateTime.Now,
                    DateOfExpiration = new DateTime(2022, 7, 1),
                    Description = "Simple hw"
                },
                new()
                {
                    TopicId = _topics[2].Id,
                    Name = "HW-03 Python",
                    DatePlannedStart = new DateTime(2022, 6, 15),
                    DateOfExpiration = new DateTime(2022, 7, 1),
                    Description = "Simple hw"
                },
                new()
                {
                    TopicId = _topics[3].Id,
                    Name = "HW-04 Math",
                    DatePlannedStart = new DateTime(2022, 6, 22),
                    DateOfExpiration = new DateTime(2022, 7, 25),
                    Description = "Simple hw"
                },

                new()
                {
                    TopicId = _topics[4].Id,
                    Name = "HW-01 .NET",
                    DatePlannedStart = new DateTime(2022, 6, 14),
                    DateOfExpiration = new DateTime(2022, 7, 23),
                    Description = "Simple hw"
                },
                new()
                {
                    TopicId = _topics[5].Id,
                    Name = "HW-02 .NET",
                    DatePlannedStart = new DateTime(2022, 6, 12),
                    DateOfExpiration = new DateTime(2022, 7, 5),
                    Description = "Simple hw"
                },
                new()
                {
                    TopicId = _topics[6].Id,
                    Name = "HW-03 ASP.NET",
                    DatePlannedStart = new DateTime(2022, 6, 14),
                    DateOfExpiration = new DateTime(2022, 7, 23),
                    Description = "Simple hw"
                },
                new()
                {
                    TopicId = _topics[7].Id,
                    Name = "HW-04 Redis",
                    DatePlannedStart = new DateTime(2022, 6, 14),
                    DateOfExpiration = new DateTime(2022, 7, 23),
                    Description = "Simple hw"
                }
            };
        }

        public static List<TaskAnswer> GetTaskAnswers()
        {
            var answers = new List<TaskAnswer>();
            for (int i = 0; i < _students.Count; i++)
            {
                answers.Add(new TaskAnswer()
                {
                    Answer = "My answer",
                    HomeTaskId = _homeTasks[0].TopicId,
                    DateOfAnswer = DateTime.Now,
                    StudentId = _students[i].Id
                });
                answers.Add(new TaskAnswer()
                {
                    Answer = "My answer",
                    HomeTaskId = _homeTasks[1].TopicId,
                    DateOfAnswer = DateTime.Now,
                    StudentId = _students[i].Id
                });
                answers.Add(new TaskAnswer()
                {
                    Answer = "My answer",
                    HomeTaskId = _homeTasks[2].TopicId,
                    DateOfAnswer = DateTime.Now,
                    StudentId = _students[i].Id
                });
                answers.Add(new TaskAnswer()
                {
                    Answer = "My answer",
                    HomeTaskId = _homeTasks[3].TopicId,
                    DateOfAnswer = DateTime.Now,
                    StudentId = _students[i].Id
                });
            }
            return answers;
        }

        public static List<Grade> GetGrades()
        {
            var grades = new List<Grade>();
            foreach (var answer in _taskAnswers)
            {
                grades.Add(new Grade()
                {
                    Id = answer.Id,
                    Comment = "Some comment",
                    Value = Random.Shared.Next(10, 100)
                });
            }

            return grades;
        }
    }
}

