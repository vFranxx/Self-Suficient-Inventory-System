namespace Self_Suficient_Inventory_System.Shared.DTOs.Role
{
    public class RoleDTO
    {
        public required string UserIdentifier { get; set; } // Puede ser ID o Email
        public required string RoleName { get; set; }
    }
}
