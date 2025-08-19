using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAnMangMayTinh.Migrations
{
    /// <inheritdoc />
    public partial class CapNhat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiaoViens",
                columns: table => new
                {
                    ID_GV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaoViens", x => x.ID_GV);
                });

            migrationBuilder.CreateTable(
                name: "KyThis",
                columns: table => new
                {
                    ID_KyThi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyThis", x => x.ID_KyThi);
                });

            migrationBuilder.CreateTable(
                name: "Lops",
                columns: table => new
                {
                    MaLop = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLop = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lops", x => x.MaLop);
                });

            migrationBuilder.CreateTable(
                name: "MonThis",
                columns: table => new
                {
                    ID_Mon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonThis", x => x.ID_Mon);
                });

            migrationBuilder.CreateTable(
                name: "PhongThis",
                columns: table => new
                {
                    ID_Phong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongChoNgoi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongThis", x => x.ID_Phong);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    ID_TB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDang = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.ID_TB);
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    ID_SV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaLop = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.ID_SV);
                    table.ForeignKey(
                        name: "FK_SinhViens_Lops_MaLop",
                        column: x => x.MaLop,
                        principalTable: "Lops",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichThis",
                columns: table => new
                {
                    ID_Lich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_KyThi = table.Column<int>(type: "int", nullable: false),
                    ID_Mon = table.Column<int>(type: "int", nullable: false),
                    ID_Phong = table.Column<int>(type: "int", nullable: false),
                    NgayThi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioThi = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichThis", x => x.ID_Lich);
                    table.ForeignKey(
                        name: "FK_LichThis_KyThis_ID_KyThi",
                        column: x => x.ID_KyThi,
                        principalTable: "KyThis",
                        principalColumn: "ID_KyThi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichThis_MonThis_ID_Mon",
                        column: x => x.ID_Mon,
                        principalTable: "MonThis",
                        principalColumn: "ID_Mon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichThis_PhongThis_ID_Phong",
                        column: x => x.ID_Phong,
                        principalTable: "PhongThis",
                        principalColumn: "ID_Phong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_SV = table.Column<int>(type: "int", nullable: true),
                    ID_GV = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.TenDangNhap);
                    table.CheckConstraint("CK_Loai", "Loai IN ('admin', 'sinhvien', 'giaovien')");
                    table.ForeignKey(
                        name: "FK_TaiKhoans_GiaoViens_ID_GV",
                        column: x => x.ID_GV,
                        principalTable: "GiaoViens",
                        principalColumn: "ID_GV");
                    table.ForeignKey(
                        name: "FK_TaiKhoans_SinhViens_ID_SV",
                        column: x => x.ID_SV,
                        principalTable: "SinhViens",
                        principalColumn: "ID_SV");
                });

            migrationBuilder.CreateTable(
                name: "DangKys",
                columns: table => new
                {
                    ID_DK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_SV = table.Column<int>(type: "int", nullable: false),
                    ID_Lich = table.Column<int>(type: "int", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKys", x => x.ID_DK);
                    table.ForeignKey(
                        name: "FK_DangKys_LichThis_ID_Lich",
                        column: x => x.ID_Lich,
                        principalTable: "LichThis",
                        principalColumn: "ID_Lich",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangKys_SinhViens_ID_SV",
                        column: x => x.ID_SV,
                        principalTable: "SinhViens",
                        principalColumn: "ID_SV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhanCongs",
                columns: table => new
                {
                    ID_PC = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_GV = table.Column<int>(type: "int", nullable: false),
                    ID_Lich = table.Column<int>(type: "int", nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCongs", x => x.ID_PC);
                    table.CheckConstraint("CK_VaiTro", "VaiTro IN ('Ra đề', 'Coi thi')");
                    table.ForeignKey(
                        name: "FK_PhanCongs_GiaoViens_ID_GV",
                        column: x => x.ID_GV,
                        principalTable: "GiaoViens",
                        principalColumn: "ID_GV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhanCongs_LichThis_ID_Lich",
                        column: x => x.ID_Lich,
                        principalTable: "LichThis",
                        principalColumn: "ID_Lich",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_ID_Lich",
                table: "DangKys",
                column: "ID_Lich");

            migrationBuilder.CreateIndex(
                name: "IX_DangKys_ID_SV",
                table: "DangKys",
                column: "ID_SV");

            migrationBuilder.CreateIndex(
                name: "IX_LichThis_ID_KyThi",
                table: "LichThis",
                column: "ID_KyThi");

            migrationBuilder.CreateIndex(
                name: "IX_LichThis_ID_Mon",
                table: "LichThis",
                column: "ID_Mon");

            migrationBuilder.CreateIndex(
                name: "IX_LichThis_ID_Phong",
                table: "LichThis",
                column: "ID_Phong");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongs_ID_GV",
                table: "PhanCongs",
                column: "ID_GV");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongs_ID_Lich",
                table: "PhanCongs",
                column: "ID_Lich");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaLop",
                table: "SinhViens",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_ID_GV",
                table: "TaiKhoans",
                column: "ID_GV");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_ID_SV",
                table: "TaiKhoans",
                column: "ID_SV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKys");

            migrationBuilder.DropTable(
                name: "PhanCongs");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "ThongBaos");

            migrationBuilder.DropTable(
                name: "LichThis");

            migrationBuilder.DropTable(
                name: "GiaoViens");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "KyThis");

            migrationBuilder.DropTable(
                name: "MonThis");

            migrationBuilder.DropTable(
                name: "PhongThis");

            migrationBuilder.DropTable(
                name: "Lops");
        }
    }
}
