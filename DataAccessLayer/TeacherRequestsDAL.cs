using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.Collections;
using Objects;
using MongoDB.Driver.Builders;
using Constants;

namespace DataAccessLayer
{
    public class TeacherRequestsDAL
    {
        public void InsertPendingTeacherRequest(TeacherRequest request)
        {
            TeacherRequestsCollection.Collection.Insert<TeacherRequest>(request);
        }

        public void SolveRequest(TeacherRequest request)
        {
            var faculty = UsersCollection.Collection.FindOneByIdAs<Faculty>(request.FacultyUsername);
            var teacher = UsersCollection.Collection.FindOneByIdAs<Teacher>(request.TeacherUsername);
            if (faculty != null && teacher != null)
            {
                if(faculty.Teachers == null)
                    faculty.Teachers = new List<string>();

                faculty.Teachers.Add(request.TeacherUsername);
                var removeQuery = Query.EQ("_id", request._id);
                TeacherRequestsCollection.Collection.Remove(removeQuery);
            }
        }

        public List<TeacherRequest> GetRequestsByFaculty(string facultyUserName)
        {
            var requests = Query.EQ("FacultyUsername", facultyUserName);
            return TeacherRequestsCollection.Collection.FindAs<TeacherRequest>(requests).ToList();
        }
    }
}
