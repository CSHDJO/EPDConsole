namespace Chipsoft.Data.Repositories
{
    public interface IAddressRepository
    {
        void DeleteById(int id);

        IEnumerable<Address> GetAll();

        void Insert(Address address);

        void Save();

    }
}