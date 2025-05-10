namespace MFL_VisitorManagement.Dtos;

public class MenuItem
{
   public int MenuId { get; set; }
   public string MenuName { get; set; }
   public string MenuIcon { get; set; }
   public string MenuRoute { get; set; }
   public List<SubMenuItem> SubMenuItem { get; set; }

}

public class SubMenuItem
{
    public int SubMenuId { get; set; }
    public string SubMenuName { get; set; }
    public string SubMenuIcon { get; set; }
    public string SubMenuRoute { get; set; }
}
