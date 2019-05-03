using System;
using System.Globalization;
using AutoMapper;
using WebClient.Core.Entities;
using WebClient.Core.Messages;
using WebClient.Core.Requests;
using WebClient.Core.Responses;
using WebClient.Core.ViewModels;

namespace WebClient.Extensions
{
    /// <summary>
    /// Adding Profiles
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// the constructor setup a mapping.
        /// </summary>
        public MappingProfile()
        {
            // employee
            this.CreateMap<Employee, EmployeeVM>()
                .ForMember(dest => dest.MaNhanVien, opts => opts.MapFrom(src => src.Ma_NhanVien))
                .ForMember(dest => dest.HoTen, opts => opts.MapFrom(src => src.Ho_Ten))
                .ForMember(dest => dest.DiaChi, opts => opts.MapFrom(src => src.Dia_Chi))
                .ForMember(dest => dest.DienThoai, opts => opts.MapFrom(src => src.Dien_Thoai))
                .ForMember(dest => dest.NamSinh, opts => opts.MapFrom(src => src.Nam_Sinh.HasValue ? src.Nam_Sinh.Value.ToString(ConsHelper.FormatDate) : string.Empty))
                .ForMember(dest => dest.SoCMND, opts => opts.MapFrom(src => src.So_CMND))
                .ForMember(dest => dest.NgayCapCMND, opts => opts.MapFrom(src => src.NgayCap_CMND.HasValue ? src.NgayCap_CMND.Value.ToString(ConsHelper.FormatDate) : string.Empty))
                .ForMember(dest => dest.NoiCapCMND, opts => opts.MapFrom(src => src.NoiCap_CMND))
                .ForMember(dest => dest.GhiChu, opts => opts.MapFrom(src => src.Ghi_Chu));

            this.CreateMap<EmployeeVM, Employee>()
                .ForMember(dest => dest.Ma_NhanVien, opts => opts.MapFrom(src => src.MaNhanVien))
                .ForMember(dest => dest.Ho_Ten, opts => opts.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.Dia_Chi, opts => opts.MapFrom(src => src.DiaChi))
                .ForMember(dest => dest.Dien_Thoai, opts => opts.MapFrom(src => src.DienThoai))
                .ForMember(dest => dest.Nam_Sinh, opts => opts.MapFrom(src => DateTime.ParseExact(src.NamSinh, ConsHelper.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.So_CMND, opts => opts.MapFrom(src => src.SoCMND))
                .ForMember(dest => dest.NgayCap_CMND, opts => opts.MapFrom(src => DateTime.ParseExact(src.NgayCapCMND, ConsHelper.FormatDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.NoiCap_CMND, opts => opts.MapFrom(src => src.NoiCapCMND))
                .ForMember(dest => dest.Ghi_Chu, opts => opts.MapFrom(src => src.GhiChu));

            this.CreateMap<EmployeeResponse, Employee>();

            // department
            this.CreateMap<Department, DepartmentVM>();
            this.CreateMap<DepartmentVM, Department>();

            this.CreateMap<EmployeeMessage, Employee>();
            this.CreateMap<Employee, EmployeeMessage>();

            this.CreateMap<Department, DepartmentMessage>();
            this.CreateMap<DepartmentMessage, Department>();

            this.CreateMap<Account, AccountMessage>();
            this.CreateMap<AccountMessage, Account>();

            this.CreateMap<AppRequest, App>();
            this.CreateMap<App, AppResponse>();

            this.CreateMap<Feature, FeatureResponse>();
        }
    }
}