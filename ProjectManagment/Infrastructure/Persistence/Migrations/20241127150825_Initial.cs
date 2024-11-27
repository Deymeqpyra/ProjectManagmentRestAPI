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
                    { new Guid("19b32952-5213-4d09-b266-9e397ed255b6"), "Development" },
                    { new Guid("5601baac-28b7-46bb-961c-0adcff6031c4"), "Documentary" },
                    { new Guid("c8bcf643-34d5-4daa-940d-ce504ffed554"), "Learning" }
                });

            migrationBuilder.InsertData(
                table: "project_priorities",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0c4d463d-1f50-42ae-bae6-b4a2c010e984"), "Very High" },
                    { new Guid("c6c1670d-cb51-49a5-9392-70e93eb24fd4"), "Very Low" },
                    { new Guid("c9a5e8df-8171-492d-86de-e88368f0480f"), "Medium" },
                    { new Guid("facf389d-c7a0-4902-847d-72aac04b73e5"), "High" }
                });

            migrationBuilder.InsertData(
                table: "project_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("7328c628-b4a6-48d3-ba25-d906f3a045c8"), "Started" },
                    { new Guid("7e6ecfdd-d43a-4ebf-945f-e22b58a04a5d"), "Finished" },
                    { new Guid("ac7e5106-ad52-42e3-9b7c-d2996d7bbdce"), "In progress" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("4ea37203-d0fd-429b-b4bf-37558527d306"), "User" },
                    { new Guid("79ab8d1e-7bdd-4360-9275-2174e13a3b3c"), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0c438a3a-3627-4426-b666-f5bfb50454b9"), "work" },
                    { new Guid("5db3e9e2-51fd-4c1e-82e4-e9ebe350fe75"), "coding" },
                    { new Guid("5e79485c-4f8e-4ebb-a72b-bbdd801c5bc2"), "database" },
                    { new Guid("722c0bb6-4629-4e16-98ae-b9c72128a671"), "dotnet" },
                    { new Guid("7d16c256-bff5-48f9-90b1-5cfb9b1707d0"), "school" },
                    { new Guid("863de1f4-c2b5-44ac-bff4-7ea0c23de679"), "python" },
                    { new Guid("997aa338-c159-474b-8e39-00a9e501411c"), "university" },
                    { new Guid("db9a7faf-628a-4bf4-a16d-adc572fee68c"), "datastructure" },
                    { new Guid("f9ccd01b-3523-4586-8f2b-eb982bbecd51"), "petproject" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "password", "project_task_id", "role_id", "user_name" },
                values: new object[,]
                {
                    { new Guid("4c1ec16d-8d20-4e13-a0c5-b87f0e9e61e2"), "user@gmail.com", "userPass", null, new Guid("4ea37203-d0fd-429b-b4bf-37558527d306"), "user" },
                    { new Guid("9c499dc9-35c4-4702-aab8-a049e62398a9"), "admin@gmail.com", "adminPass", null, new Guid("79ab8d1e-7bdd-4360-9275-2174e13a3b3c"), "admin" }
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
