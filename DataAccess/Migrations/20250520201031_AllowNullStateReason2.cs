using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullStateReason2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
        name: "StateReason",
        table: "Articles",
        type: "nvarchar(max)",
        nullable: true);

            // Обновляем существующие статьи, чтобы установить статус по умолчанию
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
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StateName",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Черновик");
        }
    }
}
