using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class HotelDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[HotelDetails]
           ([Name]
           ,[Address1]
           ,[Address2]
           ,[City]
           ,[State]
           ,[PinCode]
           ,[GSTNumber]
           ,[PhoneNumber]
           ,[MobileNumber1]
           ,[MobileNumber2]
           ,[Email]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy]
           ,[Website])
     VALUES
           ('Paradise Peak Cottage'
           ,'No: 4/337-4, Near Kodanad, View Point,'
           ,'Gandhi Nagar, Kodanadu Post,  				'
           ,'Kothagiri'
           ,'TN'
           ,'643217'
           ,'33AAEPR5004R2ZF'
           ,'9443387348'
           ,NULL
           ,NULL
           ,'paradisepeakcottage@gmail.com'
           ,getdate()
           ,1
           ,null
           ,null
           ,'https://paradisepeakcottage.com/')
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
