using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullStateReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
        name: "StateReason",
        table: "Articles",
        type: "nvarchar(max)",
        nullable: true);

            migrationBuilder.Sql(@"
        UPDATE Articles 
        SET StateName = CASE 
                WHEN IsPublished = 1 THEN 'Опубликована' 
                ELSE 'Черновик' 
            END,
            StateReason = ''
        WHERE StateName IS NULL
    ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
       name: "StateReason",
       table: "Articles",
       type: "nvarchar(max)",
       nullable: false);
        }
    }
}
