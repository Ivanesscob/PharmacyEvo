using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyEvo.Models;
using System.Windows;

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
    }
}
