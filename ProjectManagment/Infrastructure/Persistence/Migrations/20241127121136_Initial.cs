using System;
using System.Collections.Generic;
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
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comments = table.Column<List<string>>(type: "text[]", nullable: false)
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
                    { new Guid("417784d3-6c06-4f5a-9eff-b3016c8d8fc3"), "Learning" },
                    { new Guid("6b38892a-8a4e-41b3-8cab-a4989674c3e6"), "Development" },
                    { new Guid("943396cd-3d16-4724-b4a7-d1d06c39ddf4"), "Documentary" }
                });

            migrationBuilder.InsertData(
                table: "project_priorities",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("63774e52-ef50-4802-b6d7-d3ce3c5311f7"), "Very High" },
                    { new Guid("8919cf39-66e6-4306-ad87-2b181d66b664"), "Medium" },
                    { new Guid("986fc39c-c9b3-4270-bb45-1fdb130f5864"), "Very Low" },
                    { new Guid("98800eea-7933-4c0c-a3b5-76c1ef38b920"), "High" }
                });

            migrationBuilder.InsertData(
                table: "project_statuses",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("1996468e-8fd2-4a4d-a215-30ef20f2bc80"), "Started" },
                    { new Guid("3a91d30c-5e7d-41ae-b83b-8af0801b7949"), "In progress" },
                    { new Guid("66fb2c6e-5275-4ddc-81aa-054b4fcf407e"), "Finished" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("34ccae05-c1ef-42dd-9af4-8d90eb11077d"), "Admin" },
                    { new Guid("e04f9f16-5eef-41ac-9932-cf0fb98ba32b"), "User" }
                });

            migrationBuilder.InsertData(
                table: "tags",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("560b6eee-8be5-4237-a8bb-83d28e6faf87"), "database" },
                    { new Guid("5697a060-1e86-4d0b-8230-a25f71fdb133"), "coding" },
                    { new Guid("57005c27-fd12-46b2-9fb6-f52f41c16d1e"), "datastructure" },
                    { new Guid("66399fce-b89c-43a5-ae98-403a2f5fb25d"), "work" },
                    { new Guid("9593da40-b7a1-4475-9b63-b92d213ff2dd"), "petproject" },
                    { new Guid("96025c70-8dde-47ec-b3fd-01deaa4b076c"), "school" },
                    { new Guid("a78af071-ed0d-479a-8266-1f46c1f49a96"), "university" },
                    { new Guid("a81f79c8-1565-4329-84ca-07a9113b4983"), "dotnet" },
                    { new Guid("b2a32365-fb3a-4902-8684-92020201f2bf"), "python" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "password", "project_task_id", "role_id", "user_name" },
                values: new object[,]
                {
                    { new Guid("0b409bea-3c25-4ecc-ae2f-4d3aa857b429"), "admin@gmail.com", "AdminPass", null, new Guid("34ccae05-c1ef-42dd-9af4-8d90eb11077d"), "Admin" },
                    { new Guid("15e4c5cc-e48b-4bc4-97ca-5b23a6e7e5b8"), "user@gmail.com", "UserPass", null, new Guid("e04f9f16-5eef-41ac-9932-cf0fb98ba32b"), "User" }
                });

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
                name: "fk_project_task_category_id",
                table: "project_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_project_task_project_id",
                table: "project_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_project_user_id",
                table: "project_tasks");

            migrationBuilder.DropTable(
                name: "project_users");

            migrationBuilder.DropTable(
                name: "tags_projects");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "categories");

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
        }
    }
}
