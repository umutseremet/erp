using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Common;
using ERP.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Common.Extensions
{
    public static class MappingExtensions
    {
        /// <summary>
        /// Entity koleksiyonunu DTO koleksiyonuna çevirir
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source, IMapper mapper)
        {
            return mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        /// Entity'yi DTO'ya çevirir
        /// </summary>
        public static TDestination MapTo<TDestination>(this object source, IMapper mapper)
        {
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Queryable'ı sayfalı sonuca çevirir
        /// </summary>
        public static async Task<PaginatedResult<TDestination>> ToPaginatedResultAsync<TSource, TDestination>(
            this IQueryable<TSource> source,
            PaginationDto pagination,
            IMapper mapper)
            where TSource : class
        {
            var totalCount = await source.CountAsync();

            var items = await source
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            var mappedItems = mapper.Map<List<TDestination>>(items);

            return new PaginatedResult<TDestination>(mappedItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// BaseEntity'den BaseDto'ya ortak alanları map eder
        /// </summary>
        public static TDestination MapBaseEntity<TDestination>(this BaseEntity source, IMapper mapper)
            where TDestination : BaseDto, new()
        {
            var destination = mapper.Map<TDestination>(source);
            return destination;
        }

        /// <summary>
        /// Result pattern için mapping
        /// </summary>
        public static Result<TDestination> MapResult<TSource, TDestination>(this Result<TSource> source, IMapper mapper)
        {
            if (!source.IsSuccess)
            {
                return Result<TDestination>.Failure(source.ErrorMessage ?? "Mapping hatası");
            }

            if (source.Data == null)
            {
                return Result<TDestination>.Success(default(TDestination)!);
            }

            var mappedData = mapper.Map<TDestination>(source.Data);
            return Result<TDestination>.Success(mappedData);
        }

        /// <summary>
        /// Result pattern için mapping (collection)
        /// </summary>
        public static Result<List<TDestination>> MapResultList<TSource, TDestination>(this Result<List<TSource>> source, IMapper mapper)
        {
            if (!source.IsSuccess)
            {
                return Result<List<TDestination>>.Failure(source.ErrorMessage ?? "Mapping hatası");
            }

            if (source.Data == null)
            {
                return Result<List<TDestination>>.Success(new List<TDestination>());
            }

            var mappedData = mapper.Map<List<TDestination>>(source.Data);
            return Result<List<TDestination>>.Success(mappedData);
        }

        /// <summary>
        /// PaginatedResult için mapping
        /// </summary>
        public static Result<PaginatedResult<TDestination>> MapPaginatedResult<TSource, TDestination>(
            this Result<PaginatedResult<TSource>> source,
            IMapper mapper)
        {
            if (!source.IsSuccess)
            {
                return Result<PaginatedResult<TDestination>>.Failure(source.ErrorMessage ?? "Mapping hatası");
            }

            if (source.Data == null)
            {
                return Result<PaginatedResult<TDestination>>.Success(
                    new PaginatedResult<TDestination>(new List<TDestination>(), 0, 1, 10));
            }

            var mappedItems = mapper.Map<List<TDestination>>(source.Data.Items);
            var paginatedResult = new PaginatedResult<TDestination>(
                mappedItems,
                source.Data.TotalCount,
                source.Data.PageNumber,
                source.Data.PageSize);

            return Result<PaginatedResult<TDestination>>.Success(paginatedResult);
        }

        /// <summary>
        /// String değeri safe olarak trim eder
        /// </summary>
        public static string SafeTrim(this string? value)
        {
            return value?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Enum'ı display name'e çevirir
        /// </summary>
        public static string ToDisplayName(this Enum value)
        {
            return value switch
            {
                // VehicleStatus
                Core.Enums.VehicleStatus.Available => "Müsait",
                Core.Enums.VehicleStatus.Assigned => "Atanmış",
                Core.Enums.VehicleStatus.InMaintenance => "Bakımda",
                Core.Enums.VehicleStatus.OutOfService => "Hizmet Dışı",
                Core.Enums.VehicleStatus.Inspection => "Muayenede",
                Core.Enums.VehicleStatus.Accident => "Kazalı",
                Core.Enums.VehicleStatus.Sold => "Satılmış",
                Core.Enums.VehicleStatus.Scrapped => "Hurda",

                // FuelType
                Core.Enums.FuelType.Gasoline => "Benzin",
                Core.Enums.FuelType.Diesel => "Dizel",
                Core.Enums.FuelType.LPG => "LPG",
                Core.Enums.FuelType.CNG => "CNG",
                Core.Enums.FuelType.Electric => "Elektrik",
                Core.Enums.FuelType.Hybrid => "Hibrit",

                // VehicleType
                Core.Enums.VehicleType.Car => "Otomobil",
                Core.Enums.VehicleType.Van => "Minibüs",
                Core.Enums.VehicleType.Truck => "Kamyon",
                Core.Enums.VehicleType.Bus => "Otobüs",
                Core.Enums.VehicleType.Motorcycle => "Motosiklet",

                // UserStatus
                Core.Enums.UserStatus.Active => "Aktif",
                Core.Enums.UserStatus.Inactive => "Pasif",
                Core.Enums.UserStatus.Suspended => "Askıya Alınmış",
                Core.Enums.UserStatus.Terminated => "İşten Çıkarılmış",

                // MaintenanceType
                Core.Enums.MaintenanceType.Routine => "Rutin",
                Core.Enums.MaintenanceType.Preventive => "Önleyici",
                Core.Enums.MaintenanceType.Corrective => "Düzeltici",
                Core.Enums.MaintenanceType.Emergency => "Acil",

                _ => value.ToString()
            };
        }

        /// <summary>
        /// Para formatında string'e çevirir
        /// </summary>
        public static string ToCurrencyString(this decimal value, string currency = "TRY")
        {
            return currency switch
            {
                "TRY" => $"{value:N2} ₺",
                "USD" => $"${value:N2}",
                "EUR" => $"€{value:N2}",
                _ => $"{value:N2} {currency}"
            };
        }

        /// <summary>
        /// Kilometre formatında string'e çevirir
        /// </summary>
        public static string ToKilometerString(this decimal value)
        {
            return $"{value:N0} km";
        }

        /// <summary>
        /// Yakıt miktarını formatlar
        /// </summary>
        public static string ToFuelQuantityString(this decimal value)
        {
            return $"{value:N2} lt";
        }

        /// <summary>
        /// Tarih formatını Türkçe'ye çevirir
        /// </summary>
        public static string ToTurkishDateString(this DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Tarih ve saat formatını Türkçe'ye çevirir
        /// </summary>
        public static string ToTurkishDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm");
        }

        /// <summary>
        /// Null date'i güvenli şekilde formatlar
        /// </summary>
        public static string ToTurkishDateString(this DateTime? date)
        {
            return date?.ToString("dd.MM.yyyy") ?? "-";
        }
    }
}