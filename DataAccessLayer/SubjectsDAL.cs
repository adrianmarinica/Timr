using DataAccessLayer.Collections;
using MongoDB.Bson;
using Objects;

namespace DataAccessLayer
{
    public class SubjectsDAL
    {
        public Subject GetSubject(string id)
        {
            return SubjectsCollection.Collection.FindOneByIdAs<Subject>(new ObjectId(id));
        }

        public void InsertSubject(Subject subject)
        {
            SubjectsCollection.Collection.Insert(subject);
        }
    }
}