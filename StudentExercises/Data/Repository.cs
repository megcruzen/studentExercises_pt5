using System.Collections.Generic;
using System.Data.SqlClient;
using StudentExercises.Models;


namespace StudentExercises.Data
{
    ///  An object to contain all database interactions.
    public class Repository
    {
        ///  Represents a connection to the database.
        public SqlConnection Connection
        {
            get
            {
                /// This is "address" of the database
                string _connectionString = "Server=DESKTOP-LTM1VSL\\SQLEXPRESS;Database=StudentExercises;Trusted_Connection=True;";
                return new SqlConnection(_connectionString);
            }
        }

        /************************************************************************************
         * Exercises
         ************************************************************************************/

        // Return a list of all exercises in the database
        public List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                /// Open() the connection
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {

                    /// Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, ExerciseName, ExerciseLanguage FROM Exercise";

                    /// Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    /// A list to hold the departments we retrieve from the database.
                    List<Exercise> exercises = new List<Exercise>();

                    /// Read() will return true if there's more data to read
                    while (reader.Read())
                    {

                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int exerciseNameColumnPosition = reader.GetOrdinal("ExerciseName");
                        string exerciseNameValue = reader.GetString(exerciseNameColumnPosition);

                        int exerciseLanguageColumnPosition = reader.GetOrdinal("ExerciseLanguage");
                        string exerciseLanguageValue = reader.GetString(exerciseLanguageColumnPosition);


                        /// Create a new exercise object using the data from the database.
                        Exercise exercise = new Exercise
                        {
                            Id = idValue,
                            ExerciseName = exerciseNameValue,
                            ExerciseLanguage = exerciseLanguageValue,
                        };

                        /// Add that exercise object to our list.
                        exercises.Add(exercise);

                    }

                    /// Close() the reader
                    reader.Close();

                    /// Return the list of departments who whomever called this method.
                    return exercises;

                }

            }
        }

        // Add a new exercise to the database
        public void AddExercise(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    ///cmd.CommandText = $"INSERT INTO Exercise (ExerciseName, ExerciseLanguage) Values ('{exercise.ExerciseName}', '{exercise.ExerciseLanguage}')";

                    cmd.CommandText = $@"INSERT INTO Exercise (ExerciseName, ExerciseLanguage) 
                                        VALUES (@exerciseName, @exerciseLang)";
                    
                    cmd.Parameters.Add(new SqlParameter("@exerciseName", exercise.ExerciseName));
                    cmd.Parameters.Add(new SqlParameter("@exerciseLang", exercise.ExerciseLanguage));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /************************************************************************************
         * Students
         ************************************************************************************/

        // Return a list of all students in the database
        public List<Student> GetAllStudentsWithCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT s.Id, s.FirstName, s.LastName, s.Slack, s.CohortId, c.CohortName
                                        FROM Student s INNER JOIN Cohort c
                                        ON s.CohortID = c.id";

                    /// Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    /// A list to hold the departments we retrieve from the database.
                    List<Student> students = new List<Student>();

                    /// Read() will return true if there's more data to read
                    while (reader.Read())
                    {

                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int FirstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string FirstNameValue = reader.GetString(FirstNameColumnPosition);

                        int LastNameColumnPosition = reader.GetOrdinal("LastName");
                        string LastNameValue = reader.GetString(LastNameColumnPosition);

                        int SlackColumnPosition = reader.GetOrdinal("Slack");
                        string SlackValue = reader.GetString(SlackColumnPosition);

                        int CohortColumnPosition = reader.GetOrdinal("CohortId");
                        int CohortValue = reader.GetInt32(CohortColumnPosition);


                        /// Create a new student object using the data from the database.
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Slack = reader.GetString(reader.GetOrdinal("Slack")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                        
                        students.Add(student);

                    }
                    
                    reader.Close();
                    return students;

                }

            }
        }


        /************************************************************************************
         * StudentExercises (Joiner)
         ************************************************************************************/

        // Return a list of all studentexercises in the database
        public List<StudentExercise> GetAllStudentExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT se.Id, se.StudentId, se.ExerciseId, s.FirstName, s.LastName, e.ExerciseName, e.ExerciseLanguage
                                        FROM StudentExercise se
                                        LEFT JOIN Student s
                                        ON se.StudentId = s.id
                                        LEFT JOIN Exercise e
                                        ON se.ExerciseId = e.id";

                    /// Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<StudentExercise> studentExercises = new List<StudentExercise>();

                    while (reader.Read())
                    {
                        /// Create a new StudentExercise object using the data from the database.
                        StudentExercise studentExercise = new StudentExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            Student = new Student
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            },
                            Exercise = new Exercise
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                                ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                            }
                        };

                        studentExercises.Add(studentExercise);

                    }

                    reader.Close();
                    return studentExercises;

                }

            }
        }


        //  Assign an existing exercise to an existing student.
        public void AddExerciseToStudent(StudentExercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    ///cmd.CommandText = $@"INSERT INTO StudentExercise (StudentId, ExerciseId) 
                    ///                    VALUES ({exercise.StudentId}, {exercise.ExerciseId})";

                    cmd.CommandText = $@"INSERT INTO StudentExercise (StudentId, ExerciseId) 
                                        VALUES (@studentId, @exerciseId)";

                    cmd.Parameters.Add(new SqlParameter("@studentId", exercise.StudentId));
                    cmd.Parameters.Add(new SqlParameter("@exerciseId", exercise.ExerciseId));

                    cmd.ExecuteNonQuery();
                }
            }
        }


        /************************************************************************************
         * Instructors
         ************************************************************************************/

        // Return a list of all instructors in the database
        public List<Instructor> GetAllInstructorsWithCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT i.Id, i.FirstName, i.LastName, i.Slack, i.CohortId, i.InstructorType, c.CohortName
                                        FROM Instructor i INNER JOIN Cohort c
                                        ON i.CohortId = c.id";
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    List<Instructor> instructors = new List<Instructor>();
                    
                    while (reader.Read())
                    {

                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int FirstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string FirstNameValue = reader.GetString(FirstNameColumnPosition);

                        int LastNameColumnPosition = reader.GetOrdinal("LastName");
                        string LastNameValue = reader.GetString(LastNameColumnPosition);

                        int SlackColumnPosition = reader.GetOrdinal("Slack");
                        string SlackValue = reader.GetString(SlackColumnPosition);

                        int CohortColumnPosition = reader.GetOrdinal("CohortId");
                        int CohortValue = reader.GetInt32(CohortColumnPosition);

                        int TypeColumnPosition = reader.GetOrdinal("InstructorType");
                        string TypeValue = reader.GetString(TypeColumnPosition);

                        Instructor instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Slack = reader.GetString(reader.GetOrdinal("Slack")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            InstructorType = reader.GetString(reader.GetOrdinal("InstructorType")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                        
                        instructors.Add(instructor);

                    }
                    
                    reader.Close();
                    return instructors;

                }

            }
        }

        //  Add a new Instructor to the database
        public void AddInstructor(Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    ///    cmd.CommandText = $@"INSERT INTO Instructor (FirstName, LastName, Slack, CohortId) 
                    ///                      VALUES ('{instructor.FirstName}', '{instructor.LastName}', '{instructor.Slack}', {instructor.CohortId})";
                    ///    cmd.ExecuteNonQuery();

                    cmd.CommandText = $@"INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType) 
                                      VALUES (@firstName, @lastName, @slack, @cohortId, @type)";
                    
                    cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slack", instructor.Slack));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@type", instructor.InstructorType));

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
