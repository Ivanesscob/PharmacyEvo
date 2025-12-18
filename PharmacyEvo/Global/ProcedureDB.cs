using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyEvo.Models;
using System.Windows;
using System.Collections.ObjectModel;

namespace PharmacyEvo.Global
{
    public class ProcedureDB
    {
        public static bool GetCustomer(AuthModel model)
        {
            foreach (DataRow row in GetAllCustomers().Rows)
            {
                string email = row["Email"].ToString();
                string phone = row["Phone"].ToString();
                string password = row["Password"].ToString();

                if ((model.LoginOrEmail == email || model.LoginOrEmail == phone) && model.Password == password)
                {
                    GlobalClass.CurrentUser = new User
                    {
                        UserId = Convert.ToInt32(row["CustomerId"]),
                        FullName = row["FullName"].ToString(),
                        Email = email,
                        Phone = phone,
                        Password = password,
                        RoleId = Convert.ToInt32(row["RoleId"]),
                        DateCreated = Convert.ToDateTime(row["RegistrationDate"]),
                        IsActive = Convert.ToBoolean(row["IsActive"]),
                        IsCustomer = true
                    };
                    return true;
                }
            }

            return false;
        }

        public static bool LoginEmployee(AuthModel model)
        {
            string connectionString = @"Server=localhost;Database=PharmacyDB;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable employeesTable = new DataTable();
                        adapter.Fill(employeesTable);

                        foreach (DataRow row in employeesTable.Rows)
                        {
                            string email = row["Email"].ToString();
                            string phone = row["Phone"].ToString();
                            string password = row["Password"].ToString();

                            if ((model.LoginOrEmail == email || model.LoginOrEmail == phone) && model.Password == password)
                            {
                                GlobalClass.CurrentUser = new User
                                {
                                    UserId = Convert.ToInt32(row["EmployeeId"]),
                                    FullName = row["FullName"].ToString(),
                                    Email = email,
                                    Phone = phone,
                                    Password = password,
                                    RoleId = Convert.ToInt32(row["RoleId"]),
                                    DateCreated = Convert.ToDateTime(row["HireDate"]),
                                    IsActive = Convert.ToBoolean(row["IsActive"]),
                                    IsManeger = true
                                };
                                if (GlobalClass.CurrentUser.RoleId == 1) GlobalClass.CurrentUser.IsAdmin = true;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }


        public static DataTable GetAllCustomers()
        {
            string connectionString = @"Server=localhost;Database=PharmacyDB;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("GetAllCustomers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable customersTable = new DataTable();
                        adapter.Fill(customersTable);

                        return customersTable;
                    }
                }
            }
        }

        public static bool CheckCustomerEmailExists(RegisterModel model)
        {
            foreach (DataRow row in GetAllCustomers().Rows)
            {
                string email = row["Email"].ToString();

                if (model.Email == email)
                {
                    MessageBox.Show("Пользователь с таким Email уже существует!",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }

            return false;
        }

        public static bool AddCustomer(RegisterModel model)
        {
            string connectionString = @"Server=localhost;Database=PharmacyDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("AddCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FullName", model.Name);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Phone", "");
                        cmd.Parameters.AddWithValue("@Password", model.Password);
                        cmd.Parameters.AddWithValue("@RoleId", 3);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool UpdateUser(User currentUser)
        {
            string connectionString = @"Server=localhost;Database=PharmacyDB;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (currentUser.IsCustomer)
                            cmd.CommandText = "UpdateCustomer";
                        else
                            cmd.CommandText = "UpdateEmployee";

                        if (currentUser.IsCustomer)
                            cmd.Parameters.AddWithValue("@CustomerId", currentUser.UserId);
                        else
                            cmd.Parameters.AddWithValue("@EmployeeId", currentUser.UserId);

                        cmd.Parameters.AddWithValue("@FullName", currentUser.FullName);
                        cmd.Parameters.AddWithValue("@Email", currentUser.Email);
                        cmd.Parameters.AddWithValue("@Phone", currentUser.Phone);
                        cmd.Parameters.AddWithValue("@Password", currentUser.Password);
                        cmd.Parameters.AddWithValue("@RoleId", currentUser.RoleId);
                        cmd.Parameters.AddWithValue(currentUser.IsCustomer ? "@RegistrationDate" : "@HireDate", currentUser.DateCreated);
                        cmd.Parameters.AddWithValue("@IsActive", currentUser.IsActive);

                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static string _connectionString =
        @"Server=localhost;Database=PharmacyDB;Trusted_Connection=True;";

        private static ObservableCollection<T> ExecuteProcedure<T>(
            string procedureName,
            Func<DataRow, T> map)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        foreach (DataRow row in table.Rows)
                            result.Add(map(row));
                    }
                }
            }

            return result;
        }

        public static ObservableCollection<Role> GetRoles() =>
        ExecuteProcedure("GetAllRoles", row => new Role
        {
            RoleId = (int)row["RoleId"],
            RoleName = row["RoleName"].ToString()
        });

        public static ObservableCollection<Country> GetCountries() =>
            ExecuteProcedure("GetAllCountries", row => new Country
            {
                CountryId = (int)row["CountryId"],
                CountryName = row["CountryName"].ToString()
            });

        public static ObservableCollection<Manufacturer> GetManufacturers() =>
            ExecuteProcedure("GetAllManufacturers", row => new Manufacturer
            {
                ManufacturerId = (int)row["ManufacturerId"],
                Name = row["Name"].ToString(),
                CountryId = (int)row["CountryId"]
            });

        public static ObservableCollection<Supplier> GetSuppliers() =>
            ExecuteProcedure("GetAllSuppliers", row => new Supplier
            {
                SupplierId = (int)row["SupplierId"],
                Name = row["Name"].ToString(),
                Phone = row["Phone"].ToString(),
                Address = row["Address"].ToString(),
                CountryId = (int)row["CountryId"]
            });

        public static ObservableCollection<Employee> GetEmployees() =>
            ExecuteProcedure("GetAllEmployees", row => new Employee
            {
                EmployeeId = (int)row["EmployeeId"],
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Phone = row["Phone"].ToString(),
                Password = row["Password"].ToString(),
                RoleId = (int)row["RoleId"],
                HireDate = (DateTime)row["HireDate"],
                IsActive = (bool)row["IsActive"]
            });

        public static ObservableCollection<Customer> GetCustomers() =>
            ExecuteProcedure("GetAllCustomers", row => new Customer
            {
                CustomerId = (int)row["CustomerId"],
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Phone = row["Phone"].ToString(),
                Password = row["Password"].ToString(),
                RoleId = (int)row["RoleId"],
                RegistrationDate = (DateTime)row["RegistrationDate"],
                IsActive = (bool)row["IsActive"]
            });

        public static ObservableCollection<Category> GetCategories() =>
            ExecuteProcedure("GetAllCategories", row => new Category
            {
                CategoryId = (int)row["CategoryId"],
                CategoryName = row["CategoryName"].ToString()
            });

        public static ObservableCollection<Medicine> GetMedicines() =>
            ExecuteProcedure("GetAllMedicines", row => new Medicine
            {
                MedicineId = (int)row["MedicineId"],
                Name = row["Name"].ToString(),
                CategoryId = (int)row["CategoryId"],
                ManufacturerId = (int)row["ManufacturerId"],
                Price = (decimal)row["Price"],
                IsPrescription = (bool)row["IsPrescription"]
            });

        public static ObservableCollection<MedicineBatch> GetMedicineBatches() =>
            ExecuteProcedure("GetAllMedicineBatches", row => new MedicineBatch
            {
                BatchId = (int)row["BatchId"],
                MedicineId = (int)row["MedicineId"],
                Quantity = (int)row["Quantity"],
                ExpirationDate = (DateTime)row["ExpirationDate"],
                SupplierId = (int)row["SupplierId"]
            });

        public static ObservableCollection<Supply> GetSupplies() =>
            ExecuteProcedure("GetAllSupplies", row => new Supply
            {
                SupplyId = (int)row["SupplyId"],
                SupplierId = (int)row["SupplierId"],
                SupplyDate = (DateTime)row["SupplyDate"]
            });

        public static ObservableCollection<Order> GetOrders() =>
            ExecuteProcedure("GetAllOrders", row => new Order
            {
                OrderId = (int)row["OrderId"],
                OrderDate = (DateTime)row["OrderDate"],
                CustomerId = (int)row["CustomerId"],
                EmployeeId = (int)row["EmployeeId"]
            });

        private static string connectionString = @"Server=localhost;Database=PharmacyDB;Trusted_Connection=True;";

        private static bool CheckExist(string query, SqlParameter param)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(param);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private static void ExecuteDeleteProcedure(string procedureName, SqlParameter param)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(param);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteRole(int roleId)
        {
            if (CheckExist("SELECT COUNT(*) FROM Employees WHERE RoleId=@id", new SqlParameter("@id", roleId)))
            {
                MessageBox.Show("Эта роль используется сотрудниками, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteRole", new SqlParameter("@RoleId", roleId));
        }

        public static void DeleteCountry(int countryId)
        {
            if (CheckExist("SELECT COUNT(*) FROM Manufacturers WHERE CountryId=@id", new SqlParameter("@id", countryId)) ||
                CheckExist("SELECT COUNT(*) FROM Suppliers WHERE CountryId=@id", new SqlParameter("@id", countryId)))
            {
                MessageBox.Show("Эта страна используется производителями или поставщиками, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteCountry", new SqlParameter("@CountryId", countryId));
        }

        public static void DeleteManufacturer(int manufacturerId)
        {
            if (CheckExist("SELECT COUNT(*) FROM Medicines WHERE ManufacturerId=@id", new SqlParameter("@id", manufacturerId)))
            {
                MessageBox.Show("Этот производитель используется лекарствами, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteManufacturer", new SqlParameter("@ManufacturerId", manufacturerId));
        }

        public static void DeleteSupplier(int supplierId)
        {
            if (CheckExist("SELECT COUNT(*) FROM MedicineBatches WHERE SupplierId=@id", new SqlParameter("@id", supplierId)) ||
                CheckExist("SELECT COUNT(*) FROM Supplies WHERE SupplierId=@id", new SqlParameter("@id", supplierId)))
            {
                MessageBox.Show("Этот поставщик используется партиями лекарств или поставками, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteSupplier", new SqlParameter("@SupplierId", supplierId));
        }

        public static void DeleteEmployee(int employeeId)
        {
            if (CheckExist("SELECT COUNT(*) FROM Orders WHERE EmployeeId=@id", new SqlParameter("@id", employeeId)))
            {
                MessageBox.Show("Этот сотрудник используется в заказах, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteEmployee", new SqlParameter("@EmployeeId", employeeId));
        }

        public static void DeleteCustomer(int customerId)
        {
            if (CheckExist("SELECT COUNT(*) FROM Orders WHERE CustomerId=@id", new SqlParameter("@id", customerId)))
            {
                MessageBox.Show("Этот клиент имеет заказы, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteCustomer", new SqlParameter("@CustomerId", customerId));
        }

        public static void DeleteCategory(int categoryId)
        {
            if (CheckExist("SELECT COUNT(*) FROM Medicines WHERE CategoryId=@id", new SqlParameter("@id", categoryId)))
            {
                MessageBox.Show("Эта категория используется лекарствами, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteCategory", new SqlParameter("@CategoryId", categoryId));
        }

        public static void DeleteMedicine(int medicineId)
        {
            if (CheckExist("SELECT COUNT(*) FROM MedicineBatches WHERE MedicineId=@id", new SqlParameter("@id", medicineId)) ||
                CheckExist("SELECT COUNT(*) FROM Orders WHERE OrderId IN (SELECT OrderId FROM OrderItems WHERE MedicineId=@id)", new SqlParameter("@id", medicineId)))
            {
                MessageBox.Show("Это лекарство используется партиями или заказами, удалить нельзя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExecuteDeleteProcedure("DeleteMedicine", new SqlParameter("@MedicineId", medicineId));
        }

        public static void DeleteMedicineBatch(int batchId)
        {
            ExecuteDeleteProcedure("DeleteMedicineBatch", new SqlParameter("@BatchId", batchId));
        }

        public static void DeleteSupply(int supplyId)
        {
            ExecuteDeleteProcedure("DeleteSupply", new SqlParameter("@SupplyId", supplyId));
        }

        public static void DeleteOrder(int orderId)
        {
            ExecuteDeleteProcedure("DeleteOrder", new SqlParameter("@OrderId", orderId));
        }

    }
}
