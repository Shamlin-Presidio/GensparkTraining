using CheckCardioApp.Models;
using CheckCardioApp.Services;

var service = new AppointmentService();

while (true)
{
    Console.WriteLine("1. Add Appointment");
    Console.WriteLine("2. Search Appointments");
    Console.WriteLine("3. Update Appointment");
    Console.WriteLine("4. Delete Appointment");
    Console.WriteLine("5. Exit");

    Console.Write("Choose an option: ");
    var input = Console.ReadLine();
    Console.WriteLine();

    switch (input)
    {
        case "1":
            Console.Write("Patient Name: ");
            var name = Console.ReadLine();

            Console.Write("Patient Age: ");
            int age = int.Parse(Console.ReadLine()!);

            Console.Write("Appointment Date (yyyy-MM-dd HH:mm): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);

            Console.Write("Reason: ");
            var reason = Console.ReadLine();

            var appointment = new Appointment
            {
                PatientName = name!,
                PatientAge = age,
                AppointmentDate = date,
                Reason = reason!
            };

            int id = service.Add(appointment);
            Console.WriteLine($"Appointment added with ID: {id}");
            Console.WriteLine("We'll cure you and all of us will be happy, again!");
            break;

        case "2":
            var searchModel = new AppointmentSearchModel();

            Console.Write("Search by Name (optional): ");
            var sName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(sName))
                searchModel.PatientName = sName;

            Console.Write("Search by Date (yyyy-MM-dd, optional): ");
            var sDate = Console.ReadLine();
            if (DateTime.TryParse(sDate, out var parsedDate))
                searchModel.AppointmentDate = parsedDate;

            Console.Write("Min Age (optional): ");
            var minAgeInput = Console.ReadLine();
            Console.Write("Max Age (optional): ");
            var maxAgeInput = Console.ReadLine();

            if (int.TryParse(minAgeInput, out int minAge) && int.TryParse(maxAgeInput, out int maxAge))
                searchModel.AgeRange = new Range<int> { MinVal = minAge, MaxVal = maxAge };

            var results = service.Search(searchModel);

            Console.WriteLine("Matching Appointments:");
            foreach (var appt in results)
            {
                Console.WriteLine($"ID: {appt.Id}, Name: {appt.PatientName}, Age: {appt.PatientAge}, Date: {appt.AppointmentDate}, Reason: {appt.Reason}");
            }
            break;

        case "3":
            Console.Write("Enter Appointment ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int updateId))
            {
                var existing = service.GetById(updateId);
                if (existing != null)
                {
                    Console.Write("New Name (leave blank to keep): ");
                    var newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                        existing.PatientName = newName;

                    Console.Write("New Age (leave blank to keep): ");
                    var newAge = Console.ReadLine();
                    if (int.TryParse(newAge, out int updatedAge))
                        existing.PatientAge = updatedAge;

                    Console.Write("New Date (yyyy-MM-dd HH:mm, blank to keep): ");
                    var newDate = Console.ReadLine();
                    if (DateTime.TryParse(newDate, out var updatedDate))
                        existing.AppointmentDate = updatedDate;

                    Console.Write("New Reason (leave blank to keep): ");
                    var newReason = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newReason))
                        existing.Reason = newReason;

                    service.Update(existing);
                    Console.WriteLine("Appointment updated.");
                }
                else
                {
                    Console.WriteLine("Appointment not found.");
                }
            }
            break;

        case "4":
            Console.Write("Enter Appointment ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int deleteId))
            {
                service.Delete(deleteId);
                Console.WriteLine("Appointment deleted.");
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            break;

        case "5":
            return;

        default:
            Console.WriteLine("Invalid option");
            break;
    }

    Console.WriteLine();
}
