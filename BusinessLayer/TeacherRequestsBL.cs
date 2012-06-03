using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using Objects;

namespace BusinessLogic
{
    public class TeacherRequestsBL
    {
        private TeacherRequestsDAL dal
        {
            get
            {
                return new TeacherRequestsDAL();
            }
        }

        public void InsertPendingTeacherRequest(TeacherRequest request)
        {
            dal.InsertPendingTeacherRequest(request);
        }

        public void SolveRequest(TeacherRequest request)
        {
            dal.SolveRequest(request);
        }

        public List<TeacherRequest> GetRequestsByFaculty(string facultyUserName)
        {
            return dal.GetRequestsByFaculty(facultyUserName);
        }
    }
}
