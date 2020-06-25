namespace ElevenNote.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOwnerIdColumnToCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "OwnerId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "OwnerId");
        }
    }
}
