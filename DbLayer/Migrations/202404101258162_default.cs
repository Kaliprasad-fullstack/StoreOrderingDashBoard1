namespace DbLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _default : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE Tickets ADD CONSTRAINT DF_TicketStatus DEFAULT 'Open' FOR TicketStatus");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE Tickets DROP CONSTRAINT DF_TicketStatus");
        }
    }
}
