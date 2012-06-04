using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using Objects;

namespace BusinessLogic
{
    public class SubjectsBL
    {
        private SubjectsDAL dal
        {
            get
            {
                return new SubjectsDAL();
            }
        }

        public Subject GetSubject(string id)
        {
            return dal.GetSubject(id);
        }

        public void InsertSubject(Subject subject)
        {
            dal.InsertSubject(subject);
        }
    }
}
