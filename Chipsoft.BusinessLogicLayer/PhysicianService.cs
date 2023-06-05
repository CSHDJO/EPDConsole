using Chipsoft.Data;
using Chipsoft.Data.Repositories;

namespace Chipsoft.BusinessLogicLayer
{
    // We could create Unit Test projects parallel to the existing projects, for example, named Chipsoft.BusinessLogicLayer.Test
    public  class PhysicianService : IPhysicianService
    {
        private AddressRepository _addressRepository;
        private AppointmentRepository _appointmentRepository;
        private PatientRepository _patientRepository;
        private PhysicianRepository _physicianRepository;

        public PhysicianService()
        {
            EPDDbContext dbContext = new EPDDbContext();
            MakeRepositories(dbContext);
        }

        public PhysicianService(EPDDbContext dbContext)
        {
            MakeRepositories(dbContext);
        }

        private void MakeRepositories(EPDDbContext dbContext)
        {
            _addressRepository = new AddressRepository(dbContext);
            _appointmentRepository = new AppointmentRepository(dbContext);
            _patientRepository = new PatientRepository(dbContext);
            _physicianRepository = new PhysicianRepository(dbContext);
        }

        private Address AddAddress(string land, string stad, string straat, string huisnummer)
        {
            Address address = new Address();
            address.Country = land;
            address.City = stad;
            address.Street = straat;
            address.HouseNumber = huisnummer;
            _addressRepository.Insert(address);
            _addressRepository.Save();
            return address;
        }

        public void AddPhysician(string naam, string land, string stad, string straat, string huisnummer)
        {
            Address address = AddAddress(land, stad, straat, huisnummer);

            Physician physician = new Physician();
            physician.Name = naam;
            physician.AddressID = address.AddressID;
            _physicianRepository.Insert(physician);
            _physicianRepository.Save();
        }

        public void DeletePhysician(string naam)
        {
            var physicians = _physicianRepository.GetAll();

            foreach (var p in physicians)
            {
                if (p.Name == naam)
                {
                    _physicianRepository.DeleteById(p.PhysicianID);
                    _addressRepository.DeleteById(p.AddressID);
                    _physicianRepository.Save();
                    _addressRepository.Save();
                }
            }
        }

        public void AddAppointment(string naamArts, string naamPatient, string beschrijving)
        {
            var physicians = _physicianRepository.GetAll();
            Physician? physician = null;
            foreach (var p in physicians)
            {
                if (p.Name == naamArts)
                {
                    physician = p;
                }
            }

            var patients = _patientRepository.GetAll();
            Patient? patient = null;
            foreach (var p in patients)
            {
                if (p.Name == naamPatient)
                {
                    patient = p;
                }
            }

            if (physician != null && patient != null)
            {
                Appointment appointment = new Appointment();
                appointment.Description = beschrijving;
                appointment.PatientID = patient.PatientID;
                appointment.PhysicianID = physician.PhysicianID;
                _appointmentRepository.Insert(appointment);
                _appointmentRepository.Save();
            }
        }

        public List<AppointmentItem> ShowAppointment()
        {
            List<AppointmentItem> list = new List<AppointmentItem>();
            var allAppointments = _appointmentRepository.GetAll();

            foreach (var appointment in allAppointments)
            {
                Physician? physician = _physicianRepository.GetById(appointment.PhysicianID);
                Patient? patient = _patientRepository.GetById(appointment.PatientID);

                if (physician != null && patient != null)
                {
                    AppointmentItem item = new AppointmentItem();
                    item.Description = appointment.Description;
                    item.PatientName = patient.Name;
                    item.PhysicianName = physician.Name;
                    list.Add(item);
                }
            }
            return list;
        }

        public bool PhysiciansExist()
        {
            return _physicianRepository.GetAll().Any();
        }

        public bool PhysicianExists(string naamArts)
        {
            bool result;            
            var all = _physicianRepository.GetAll();
            result = all.Any(p => p.Name == naamArts);
            return result;
        }

        public List<AppointmentItem> ShowAppointmentForPhysician(string naam)
        {
            List<AppointmentItem> list = new List<AppointmentItem>();
            var allAppointments = _appointmentRepository.GetAll();

            foreach (var appointment in allAppointments)
            {
                Physician? physician = _physicianRepository.GetById(appointment.PhysicianID);
                Patient? patient = _patientRepository.GetById(appointment.PatientID);

                if (physician != null && patient != null && physician.Name == naam)
                {
                    AppointmentItem item = new AppointmentItem();
                    item.Description = appointment.Description;
                    item.PatientName = patient.Name;
                    item.PhysicianName = physician.Name;
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
