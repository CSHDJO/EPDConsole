namespace Chipsoft.Data.Repositories
{
    public interface IPhysicianRepository
    {
        void DeleteById(int id);

        IEnumerable<Physician> GetAll();

        void Insert(Physician physician);

        void Save();

    }
}