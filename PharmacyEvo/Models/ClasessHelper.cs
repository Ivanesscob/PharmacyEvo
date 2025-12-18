using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyEvo.Models
{
    #region Roles
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    #endregion

    #region Countries
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    #endregion

    #region Manufacturers
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
    #endregion

    #region Suppliers
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
    }
    #endregion

    #region Employees
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region Customers
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region Categories
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    #endregion

    #region Medicines
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public decimal Price { get; set; }
        public bool IsPrescription { get; set; }
        public string ImagePath { get; set; }
    }
    #endregion

    #region MedicineBatches
    public class MedicineBatch
    {
        public int BatchId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int SupplierId { get; set; }
    }
    #endregion

    #region Supplies
    public class Supply
    {
        public int SupplyId { get; set; }
        public int SupplierId { get; set; }
        public DateTime SupplyDate { get; set; }
    }
    #endregion

    #region Orders
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
    }
    #endregion

    //#region OrderItems
    //public class OrderItem
    //{
    //    public int OrderItemId { get; set; }
    //    public int OrderId { get; set; }
    //    public int MedicineId { get; set; }
    //    public int Quantity { get; set; }
    //    public decimal Price { get; set; }
    //}
    //#endregion
}
