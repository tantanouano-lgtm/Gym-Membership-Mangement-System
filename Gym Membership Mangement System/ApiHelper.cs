using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiHelper
{
    private static readonly HttpClient client = new HttpClient();
    private const string BaseUrl = "http://localhost/GMS/functions/api/";

    // GET - Fetch all members
    public static async Task<string> GetMembers()
    {
        HttpResponseMessage response = await client.GetAsync(BaseUrl + "members.php");
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Add new member
    public static async Task<string> AddMember(object memberData)
    {
        string json = JsonConvert.SerializeObject(memberData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "members.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // PUT - Update member
    public static async Task<string> UpdateMember(object memberData)
    {
        string json = JsonConvert.SerializeObject(memberData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(BaseUrl + "members.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE - Delete member
    public static async Task<string> DeleteMember(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "members.php?id=" + id);
        return await response.Content.ReadAsStringAsync();
    }

    // GET - Fetch all staff ← FIXED: was staff.php, now get_staff.php
    public static async Task<string> GetStaff()
    {
        HttpResponseMessage response = await client.GetAsync(BaseUrl + "get_staff.php");
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Add new staff
    public static async Task<string> AddStaff(object staffData)
    {
        string json = JsonConvert.SerializeObject(staffData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "add_staff.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Add new trainer
    public static async Task<string> AddTrainer(object trainerData)
    {
        string json = JsonConvert.SerializeObject(trainerData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "trainers.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // GET - Fetch all equipment
    public static async Task<string> GetEquipment()
    {
        HttpResponseMessage response = await client.GetAsync(
            "http://localhost/GMS/api/equipment.php");
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Admin/Member Login
    public static async Task<string> Login(object loginData)
    {
        string json = JsonConvert.SerializeObject(loginData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "login.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Register new member account
    public static async Task<string> RegisterMember(object registerData)
    {
        string json = JsonConvert.SerializeObject(registerData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "register_admin.php", content);
        return await response.Content.ReadAsStringAsync();
    }
    // DELETE equipment
    public static async Task<string> DeleteEquipment(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync(
            "http://localhost/GMS/api/equipment.php?id=" + id);
        return await response.Content.ReadAsStringAsync();
    }
}