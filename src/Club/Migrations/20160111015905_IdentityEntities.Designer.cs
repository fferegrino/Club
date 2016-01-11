using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Club.Models;

namespace Club.Migrations
{
    [DbContext(typeof(ClubContext))]
    [Migration("20160111015905_IdentityEntities")]
    partial class IdentityEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Club.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");
                });
        }
    }
}
