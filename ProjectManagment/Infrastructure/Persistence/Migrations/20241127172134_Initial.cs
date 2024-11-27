using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project_priorities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_priorities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    posted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc',now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project_tasks",
                columns: table => new
                {
                    task_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    short_description = table.Column<string>(type: "text", nullable: false),
                    is_finished = table.Column<bool>(type: "boolean", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "fk_project_task_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "varchar(255)", nullable: false),
                    password = table.Column<string>(type: "varchar(255)", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_task_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_project_task_id",
                        column: x => x.project_task_id,
                        principalTable: "project_tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc',now())"),
                    project_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_priority_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.project_id);
                    table.ForeignKey(
                        name: "fk_project_priority_id",
                        column: x => x.project_priority_id,
                        principalTable: "project_priorities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_project_status_id",
                        column: x => x.project_status_id,
                        principalTable: "project_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_project_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_users",
                columns: table => new
                {
                    project_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_users", x => x.project_user_id);
                    table.ForeignKey(
                        name: "fk_projectuser_project",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_projectuser_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tags_projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags_projects", x => x.id);
                    table.ForeignKey(
                        name: "fk_tagprojects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tagprojects_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0dda3937-9365-4191-ab6f-19419ad76aae"), "Learning" },
                    { new Guid("260f5527-35cc-40b3-b6ff-554cadd22f9f"), "Development" },
                    { new Guid("f2fb0bf5-4d50-4451-a97e-d30c79e1e8bd"), "Documentary" }
                });

            migrationBuilder.InsertData(
                table: "project_priorities",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1b077375-f3a3-4ce2-b224-cc8c903e50c6"), "Medium" },
                    { new Guid("1d02df9b-c15d-4b40-b027-8fb83ce1c128"), "High" },
                    { new Guid("4ab5d04b-3825-46cb-be02-3d56a69867f3"), "Very Low" },
                    { new Guid("c2852904-ee21-4fda-930f-43bd583ff4ab"), "Very High" }
                });

            migrationBuilder.InsertData(
                table: "project_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("2d9728a1-ed70-4839-9e37-62659d4657ed"), "In progress" },
                    { new Guid("c53e2b83-be37-4df4-bc6e-0b5661ac9c35"), "Started" },
                    { new Guid("e2ca6df9-c001-4dc0-92e8-956853a87095"), "Finished" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0392369f-874b-4689-8e3b-7521d2a5074a"), "User" },
                    { new Guid("7c9fb46e-1580-427c-ac38-b4fbc2d81793"), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("04252352-9d7b-4099-8efc-8da1c3bceff3"), "datastructure" },
                    { new Guid("118d6601-b720-4316-8e11-8946c6be7aa6"), "database" },
                    { new Guid("2657c14d-593b-4be0-aead-da191e38b379"), "school" },
                    { new Guid("5c488cbf-cf4c-4fd9-b6b7-626a3f9a4ca1"), "dotnet" },
                    { new Guid("7754984f-42c7-4857-aaa0-458140482e16"), "coding" },
                    { new Guid("98eb0756-17eb-419b-837f-6601af1accc1"), "petproject" },
                    { new Guid("a555194d-fede-4ae5-a490-80466dc3445f"), "university" },
                    { new Guid("b9ac75f0-d282-4c25-a4a3-edb489c64a2c"), "work" },
                    { new Guid("e63264ef-d491-47f3-ade3-842f9a49cd8b"), "python" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "password", "project_task_id", "role_id", "user_name" },
                values: new object[,]
                {
                    { new Guid("103145f9-0cab-413c-9135-0223c0b48dfc"), "user@gmail.com", "$2a$10$j2yMWD8agYpX9DeDn1aG/OkqKjRF4sNhvTo9p10NkReFyv6P.OEiq", null, new Guid("0392369f-874b-4689-8e3b-7521d2a5074a"), "user" },
                    { new Guid("ab0d5cce-7815-440d-a89e-1aeb73e9af00"), "admin@gmail.com", "$2a$10$6jiVstkgQ2dmsepl9.8n3.eU68scTE2BMFWWpABOk2/u2sSSFdOxO", null, new Guid("7c9fb46e-1580-427c-ac38-b4fbc2d81793"), "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_comments_project_id",
                table: "comments",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_tasks_category_id",
                table: "project_tasks",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_tasks_project_id",
                table: "project_tasks",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_tasks_user_id",
                table: "project_tasks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_users_project_id",
                table: "project_users",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_users_user_id",
                table: "project_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_projects_project_priority_id",
                table: "projects",
                column: "project_priority_id");

            migrationBuilder.CreateIndex(
                name: "ix_projects_project_status_id",
                table: "projects",
                column: "project_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_projects_user_id",
                table: "projects",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_projects_project_id",
                table: "tags_projects",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_tags_projects_tag_id",
                table: "tags_projects",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_project_task_id",
                table: "users",
                column: "project_task_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_project_comments",
                table: "comments",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "project_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_comments",
                table: "comments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_project_task_project_id",
                table: "project_tasks",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "project_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_project_user_id",
                table: "project_tasks",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_task_project_id",
                table: "project_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_project_user_id",
                table: "project_tasks");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "project_users");

            migrationBuilder.DropTable(
                name: "tags_projects");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "project_priorities");

            migrationBuilder.DropTable(
                name: "project_statuses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "project_tasks");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
