﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241127172134_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Categories.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("260f5527-35cc-40b3-b6ff-554cadd22f9f"),
                            Name = "Development"
                        },
                        new
                        {
                            Id = new Guid("f2fb0bf5-4d50-4451-a97e-d30c79e1e8bd"),
                            Name = "Documentary"
                        },
                        new
                        {
                            Id = new Guid("0dda3937-9365-4191-ab6f-19419ad76aae"),
                            Name = "Learning"
                        });
                });

            modelBuilder.Entity("Domain.Comments.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("content");

                    b.Property<DateTime>("PostedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("posted_at")
                        .HasDefaultValueSql("timezone('utc',now())");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_comments_project_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_comments_user_id");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("Domain.Priorities.ProjectPriority", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_project_priorities");

                    b.ToTable("project_priorities", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("4ab5d04b-3825-46cb-be02-3d56a69867f3"),
                            Name = "Very Low"
                        },
                        new
                        {
                            Id = new Guid("1b077375-f3a3-4ce2-b224-cc8c903e50c6"),
                            Name = "Medium"
                        },
                        new
                        {
                            Id = new Guid("1d02df9b-c15d-4b40-b027-8fb83ce1c128"),
                            Name = "High"
                        },
                        new
                        {
                            Id = new Guid("c2852904-ee21-4fda-930f-43bd583ff4ab"),
                            Name = "Very High"
                        });
                });

            modelBuilder.Entity("Domain.ProjectUsers.ProjectUser", b =>
                {
                    b.Property<Guid>("ProjectUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("project_user_id");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("ProjectUserId")
                        .HasName("pk_project_users");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_project_users_project_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_project_users_user_id");

                    b.ToTable("project_users", (string)null);
                });

            modelBuilder.Entity("Domain.Projects.Project", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("description");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update")
                        .HasDefaultValueSql("timezone('utc',now())");

                    b.Property<Guid>("ProjectPriorityId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_priority_id");

                    b.Property<Guid>("ProjectStatusId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_status_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("ProjectId")
                        .HasName("pk_projects");

                    b.HasIndex("ProjectPriorityId")
                        .HasDatabaseName("ix_projects_project_priority_id");

                    b.HasIndex("ProjectStatusId")
                        .HasDatabaseName("ix_projects_project_status_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_projects_user_id");

                    b.ToTable("projects", (string)null);
                });

            modelBuilder.Entity("Domain.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("0392369f-874b-4689-8e3b-7521d2a5074a"),
                            Name = "User"
                        },
                        new
                        {
                            Id = new Guid("7c9fb46e-1580-427c-ac38-b4fbc2d81793"),
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Domain.Statuses.ProjectStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_project_statuses");

                    b.ToTable("project_statuses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("c53e2b83-be37-4df4-bc6e-0b5661ac9c35"),
                            Name = "Started"
                        },
                        new
                        {
                            Id = new Guid("2d9728a1-ed70-4839-9e37-62659d4657ed"),
                            Name = "In progress"
                        },
                        new
                        {
                            Id = new Guid("e2ca6df9-c001-4dc0-92e8-956853a87095"),
                            Name = "Finished"
                        });
                });

            modelBuilder.Entity("Domain.Tags.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.ToTable("tags", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("2657c14d-593b-4be0-aead-da191e38b379"),
                            Name = "school"
                        },
                        new
                        {
                            Id = new Guid("5c488cbf-cf4c-4fd9-b6b7-626a3f9a4ca1"),
                            Name = "dotnet"
                        },
                        new
                        {
                            Id = new Guid("b9ac75f0-d282-4c25-a4a3-edb489c64a2c"),
                            Name = "work"
                        },
                        new
                        {
                            Id = new Guid("98eb0756-17eb-419b-837f-6601af1accc1"),
                            Name = "petproject"
                        },
                        new
                        {
                            Id = new Guid("7754984f-42c7-4857-aaa0-458140482e16"),
                            Name = "coding"
                        },
                        new
                        {
                            Id = new Guid("e63264ef-d491-47f3-ade3-842f9a49cd8b"),
                            Name = "python"
                        },
                        new
                        {
                            Id = new Guid("a555194d-fede-4ae5-a490-80466dc3445f"),
                            Name = "university"
                        },
                        new
                        {
                            Id = new Guid("118d6601-b720-4316-8e11-8946c6be7aa6"),
                            Name = "database"
                        },
                        new
                        {
                            Id = new Guid("04252352-9d7b-4099-8efc-8da1c3bceff3"),
                            Name = "datastructure"
                        });
                });

            modelBuilder.Entity("Domain.TagsProjects.TagsProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid")
                        .HasColumnName("tag_id");

                    b.HasKey("Id")
                        .HasName("pk_tags_projects");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_tags_projects_project_id");

                    b.HasIndex("TagId")
                        .HasDatabaseName("ix_tags_projects_tag_id");

                    b.ToTable("tags_projects", (string)null);
                });

            modelBuilder.Entity("Domain.Tasks.ProjectTask", b =>
                {
                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid")
                        .HasColumnName("task_id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean")
                        .HasColumnName("is_finished");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("short_description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("TaskId")
                        .HasName("pk_project_tasks");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_project_tasks_category_id");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_project_tasks_project_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_project_tasks_user_id");

                    b.ToTable("project_tasks", (string)null);
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<Guid?>("ProjectTaskId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_task_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("ProjectTaskId")
                        .HasDatabaseName("ix_users_project_task_id");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_users_role_id");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("103145f9-0cab-413c-9135-0223c0b48dfc"),
                            Email = "user@gmail.com",
                            Password = "$2a$10$j2yMWD8agYpX9DeDn1aG/OkqKjRF4sNhvTo9p10NkReFyv6P.OEiq",
                            RoleId = new Guid("0392369f-874b-4689-8e3b-7521d2a5074a"),
                            UserName = "user"
                        },
                        new
                        {
                            Id = new Guid("ab0d5cce-7815-440d-a89e-1aeb73e9af00"),
                            Email = "admin@gmail.com",
                            Password = "$2a$10$6jiVstkgQ2dmsepl9.8n3.eU68scTE2BMFWWpABOk2/u2sSSFdOxO",
                            RoleId = new Guid("7c9fb46e-1580-427c-ac38-b4fbc2d81793"),
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Domain.Comments.Comment", b =>
                {
                    b.HasOne("Domain.Projects.Project", "Project")
                        .WithMany("Comments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_comments");

                    b.HasOne("Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_user_comments");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.ProjectUsers.ProjectUser", b =>
                {
                    b.HasOne("Domain.Projects.Project", "Project")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_projectuser_project");

                    b.HasOne("Domain.Users.User", "User")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_projectuser_user");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Projects.Project", b =>
                {
                    b.HasOne("Domain.Priorities.ProjectPriority", "ProjectPriority")
                        .WithMany()
                        .HasForeignKey("ProjectPriorityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_priority_id");

                    b.HasOne("Domain.Statuses.ProjectStatus", "ProjectStatus")
                        .WithMany()
                        .HasForeignKey("ProjectStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_status_id");

                    b.HasOne("Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_user_id");

                    b.Navigation("ProjectPriority");

                    b.Navigation("ProjectStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.TagsProjects.TagsProject", b =>
                {
                    b.HasOne("Domain.Projects.Project", "Project")
                        .WithMany("TagsProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_tagprojects_project_id");

                    b.HasOne("Domain.Tags.Tag", "Tag")
                        .WithMany("TagsProjects")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_tagprojects_tag_id");

                    b.Navigation("Project");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Domain.Tasks.ProjectTask", b =>
                {
                    b.HasOne("Domain.Categories.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_task_category_id");

                    b.HasOne("Domain.Projects.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_task_project_id");

                    b.HasOne("Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_project_user_id");

                    b.Navigation("Category");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.HasOne("Domain.Tasks.ProjectTask", "ProjectTask")
                        .WithMany()
                        .HasForeignKey("ProjectTaskId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_user_project_task_id");

                    b.HasOne("Domain.Roles.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_user_role_id");

                    b.Navigation("ProjectTask");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Projects.Project", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ProjectUsers");

                    b.Navigation("TagsProjects");
                });

            modelBuilder.Entity("Domain.Tags.Tag", b =>
                {
                    b.Navigation("TagsProjects");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Navigation("ProjectUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
