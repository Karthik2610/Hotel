using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class addrooms_updateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "RoomsLU",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "RoomsLU",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

			migrationBuilder.Sql(@"INSERT INTO [dbo].[RoomsLU] ([RoomName],[RoomType],[Price],[CreatedDate],[CreatedBy],[ModifiedDate],[ModifiedBy]) VALUES ('Room1','AC',3000.00,GETDATE(),'aac22322-7f9f-4b42-b415-89b7b53f582d',null,null)
INSERT INTO [dbo].[RoomsLU] ([RoomName],[RoomType],[Price],[CreatedDate],[CreatedBy],[ModifiedDate],[ModifiedBy]) VALUES ('Room2','NonAC',2500.00,GETDATE(),'aac22322-7f9f-4b42-b415-89b7b53f582d',null,null)
INSERT INTO [dbo].[RoomsLU] ([RoomName],[RoomType],[Price],[CreatedDate],[CreatedBy],[ModifiedDate],[ModifiedBy]) VALUES ('Room3','AC',2300.00,GETDATE(),'aac22322-7f9f-4b42-b415-89b7b53f582d',null,null)
INSERT INTO [dbo].[RoomsLU] ([RoomName],[RoomType],[Price],[CreatedDate],[CreatedBy],[ModifiedDate],[ModifiedBy]) VALUES ('Room4','AC',2100.00,GETDATE(),'aac22322-7f9f-4b42-b415-89b7b53f582d',null,null)
INSERT INTO [dbo].[RoomsLU] ([RoomName],[RoomType],[Price],[CreatedDate],[CreatedBy],[ModifiedDate],[ModifiedBy]) VALUES ('Room5','AC',1800.00,GETDATE(),'aac22322-7f9f-4b42-b415-89b7b53f582d',null,null)
INSERT INTO [dbo].[RoomsLU] ([RoomName],[RoomType],[Price],[CreatedDate],[CreatedBy],[ModifiedDate],[ModifiedBy]) VALUES ('Room6','AC',2200.00,GETDATE(),'aac22322-7f9f-4b42-b415-89b7b53f582d',null,null)");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "RoomsLU",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "RoomsLU",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
