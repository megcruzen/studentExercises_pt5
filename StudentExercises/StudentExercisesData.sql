INSERT INTO Cohort (CohortName) VALUES ('Day Cohort 28');
INSERT INTO Cohort (CohortName) VALUES ('Day Cohort 29');
INSERT INTO Cohort (CohortName) VALUES ('Day Cohort 30');
INSERT INTO Cohort (CohortName) VALUES ('Day Cohort 31');

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Megan', 'Cruzen', 'megcruzen', 2);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Asia', 'Carter', 'asiacarter', 2);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Mary', 'Remo', 'maryremo', 2);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Jane', 'Smith', 'janesmith', 1);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Jennifer', 'Walters', 'shulkie', 1);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Kara', 'Thrace', 'starbuck', 3);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Lee', 'Adama', 'apollo', 3);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Han', 'Solo', 'scoundrel', 4);

INSERT INTO Student (FirstName, LastName, Slack, CohortId)
VALUES ('Carol', 'Danvers', 'sparklefists', 4);


INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType)
VALUES ('Kimmy', 'Bird', 'kbird', 1, 'Junior');

INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType)
VALUES ('Andy', 'Collins', 'andycollins', 2, 'Senior');

INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType)
VALUES ('Leah', 'Hoefling', 'leah', 2, 'Junior');

INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType)
VALUES ('Madi', 'Peper', 'madi', 2, 'Junior');

INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType)
VALUES ('Steve', 'Brownlee', 'coach', 3, 'Senior');

INSERT INTO Instructor (FirstName, LastName, Slack, CohortId, InstructorType)
VALUES ('Jisie', 'David', 'jisie', 4, 'Senior');


INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('Something Django', 'Django');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('Django Mango', 'Django');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('User Defined Types', 'C#');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('Dictionaries', 'C#');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('Building DOM Components', 'Javascript');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('Building and Using an API', 'Javascript');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('State and Props', 'React');

INSERT INTO Exercise (ExerciseName, ExerciseLanguage)
VALUES ('URL Routing', 'React');


INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (1, 3);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (1, 4);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (2, 3);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (2, 4);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (3, 3);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (3, 4);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (4, 1);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (4, 2);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (5, 1);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (5, 2);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (6, 5);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (6, 6);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (7, 5);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (7, 6);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (8, 7);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (8, 8);

INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (9, 7);
INSERT INTO StudentExercise (StudentId, ExerciseId) VALUES (9, 8);


SELECT * FROM Cohort;
SELECT * FROM Instructor;
SELECT * FROM Student;
SELECT * FROM Exercise;
SELECT * FROM StudentExercise;
