using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using StudentExercises.Models;

namespace StudentExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


        /*******************
        * Get all cohorts
        *******************/
        [HttpGet]
        /*
        public List<Cohort> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, CohortName FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };
                        cohorts.Add(cohort);
                    };

                    reader.Close();
                    return cohorts;
                }
            }
        }
        */
        public List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.id AS CohortId, c.CohortName,
                                                s.Id AS StudentId, s.FirstName AS StudentFirstName, s.LastName AS StudentLastName, s.Slack AS StudentSlack,
                                                i.Id AS InstructorId, i.FirstName AS InstructorFirstName, i.LastName AS InstructorLastName, i.Slack AS InstructorSlack
                                       FROM Cohort c
                                       LEFT JOIN Student s ON s.CohortId = c.id
                                       LEFT JOIN Instructor i ON i.CohortId = c.id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();
                    while (reader.Read())
                    {
                        int cohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                        if (!cohorts.ContainsKey(cohortId))
                        {
                            Cohort newCohort = new Cohort
                            {
                                Id = cohortId,
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                                //LeadInstructor = new Instructor
                                //{
                                //    FirstName = reader.GetString(reader.GetOrdinal("InstructorFirstName")),
                                //    LastName = reader.GetString(reader.GetOrdinal("InstructorLastName"))
                                //}
                            };

                            cohorts.Add(cohortId, newCohort);
                        }
                        
                        Cohort currentCohort = cohorts[cohortId];

                        int studentId = reader.GetInt32(reader.GetOrdinal("StudentId"));
                        if (!currentCohort.StudentList.Exists(s => s.Id == studentId))
                        {
                            currentCohort.StudentList.Add(
                            new Student
                                {
                                    Id = studentId,
                                    FirstName = reader.GetString(reader.GetOrdinal("StudentFirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("StudentLastName")),
                                    Slack = reader.GetString(reader.GetOrdinal("StudentSlack"))
                            }
                            );
                        }

                        int instructorId = reader.GetInt32(reader.GetOrdinal("InstructorId"));
                        if (!currentCohort.InstructorList.Exists(i => i.Id == instructorId))
                        {
                            currentCohort.InstructorList.Add(
                            new Instructor
                                {
                                    Id = instructorId,
                                    FirstName = reader.GetString(reader.GetOrdinal("InstructorFirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("InstructorLastName")),
                                    Slack = reader.GetString(reader.GetOrdinal("InstructorSlack"))
                            }
                            );
                        }


                    }

                    reader.Close();

                    return cohorts.Values.ToList();
                }
            }
        }

        /*******************
        * Get cohort
        *******************/

        [HttpGet("{id}", Name = "GetCohort")]
        public Cohort Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, CohortName
                        FROM Cohort
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort cohort = null;

                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };
                    }
                    reader.Close();
                    return cohort;
                }
            }
        }

        /*******************
       * Create cohort
       *******************/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cohort newCohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort (CohortName)
                                        OUTPUT INSERTED.Id
                                        VALUES (@cohortName)";
                    cmd.Parameters.Add(new SqlParameter("@cohortName", newCohort.CohortName));

                    int newId = (int)cmd.ExecuteScalar(); // Expects one thing back
                    newCohort.Id = newId;
                    return CreatedAtRoute("GetCohort", new { id = newId }, newCohort);
                }
            }
        }

        /*******************
        * Edit cohort
        *******************/

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Cohort cohort)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Cohort
                                            SET CohortName = @cohortName
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@cohortName", cohort.CohortName));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ObjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /*******************
        * Delete cohort
        *******************/

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ObjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ObjectExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, CohortName
                        FROM Cohort
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}