﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using ETNA.BL;
using AutoMapper;
using ETNA.BL.LO;
using ETNA.Domain;
using ETNA.DTOs;
using ETNA.DTOs.LO;
using ETNA.Common;

namespace ETNA.WCF.LO
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "EntradasYSalidasService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione EntradasYSalidasService.svc o EntradasYSalidasService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class EntradasYSalidasService : IEntradasYSalidasService
    {
        public List<AlmacenDto> ObtenerAlmacenesPorEmpleado(int idEmpleado)
        {
            var gestorAlmacen = new GestorAlmacenes();
            var productos = gestorAlmacen.ObtenerAlmacenesPorEmpleado(idEmpleado);
            Mapper.CreateMap<Almacen, AlmacenDto>();
            var listaAlmacenes = Mapper.Map<List<Almacen>, List<AlmacenDto>>(productos);
            return listaAlmacenes;
        }

        public List<ListaSolicitudEntradaDto> ObtenerSolicitudesEntrada(int idSolicitud, int estadoSolicitud, DateTime fechaInicio,
            DateTime fechaFin, int idEmpleado, int tipoEntrada)
        {
            var gestorSolicitud = new GestorSolicitudesEntrada();
            var solicitudes = gestorSolicitud.ObtenerSolicitudesEntrada(idSolicitud, estadoSolicitud, fechaInicio, fechaFin, idEmpleado,
                tipoEntrada);
            Mapper.CreateMap<SolicitudEntrada, ListaSolicitudEntradaDto>()
                .ForMember(s => s.TipoEntrada, opts => opts.MapFrom(src => Enums.GetEnumDescription((Enums.TipoEntrada)src.TipoEntrada)))
                .ForMember(s => s.NombreEmpleado, opts => opts.MapFrom(src => src.Empleado.Nombres + ' ' + src.Empleado.Apellidos));
            var dtos = Mapper.Map<List<ListaSolicitudEntradaDto>>(solicitudes);
            return dtos;
        }

        public SolicitudEntradaDto ObtenerSolicitudEntrada(int idSolicitud)
        {
            var gestorSolicitud = new GestorSolicitudesEntrada();
            var solicitud = gestorSolicitud.ObtenerSolicitdEntrada(idSolicitud);
            Mapper.CreateMap<DetalleSolicitudEntrada, DetalleSolicitudEntradaDto>()
                .ForMember(d => d.Id, opts => opts.MapFrom(src => src.Producto.Id))
                .ForMember(d => d.NombreProducto, opts => opts.MapFrom(src => src.Producto.Nombre));
            Mapper.CreateMap<SolicitudEntrada, SolicitudEntradaDto>()
                .ForMember(s => s.TipoEntrada,
                    opts => opts.MapFrom(src => Enums.GetEnumDescription((Enums.TipoEntrada) src.TipoEntrada)))
                .ForMember(s => s.NombreEmpleado,
                    opts => opts.MapFrom(src => src.Empleado.Nombres + ' ' + src.Empleado.Apellidos))
                .ForMember(s => s.Detalle,
                    opts => opts.MapFrom(src => src.SolicitudEntradaProducto));
            var dto = Mapper.Map<SolicitudEntradaDto>(solicitud);
            return dto;
        }
        
        public bool GenerarGuiaEntrada(int idSolicitud, int idAlmacen, int idEmpleado)
        {
            var gestorGuia = new GestorGuiasEntrada();
            return gestorGuia.GenerarGuiaEntrada(idSolicitud, idAlmacen, idEmpleado);
        }

        public bool RechazarGuiaEntrada(int idSolicitud, string observaciones)
        {
            var gestorGuia = new GestorGuiasEntrada();
            return gestorGuia.RechazarGuiaEntrada(idSolicitud, observaciones);
        }

        public List<ListaSolicitudSalidaDto> ObtenerSolicitudesSalida(int idSolicitud, int estadoSolicitud, DateTime fechaInicio, DateTime fechaFin,
            int idEmpleado, string tipoSalida, string direccionEntrega, string razonSocial)
        {
            var gestorSolicitud = new GestorSolicitudesSalida();
            var solicitudes = gestorSolicitud.ObtenerSolicitudesSalida(idSolicitud, estadoSolicitud, fechaInicio,
                fechaFin, idEmpleado, tipoSalida, direccionEntrega, razonSocial);
            Mapper.CreateMap<SolicitudSalida, ListaSolicitudSalidaDto>()
                //.ForMember(s => s.TipoSalida, opts => opts.MapFrom(src => Enums.GetEnumDescription((Enums.TipoEntrada)src.TipoEntrada)))
                .ForMember(s => s.NombreEmpleado, opts => opts.MapFrom(src => src.Empleado.Nombres + ' ' + src.Empleado.Apellidos));
            var dtos = Mapper.Map<List<ListaSolicitudSalidaDto>>(solicitudes);
            return dtos;
        }

        public SolicitudSalidaDto ObtenerSolicitudSalida(int idSolicitud)
        {
            var gestorSolicitud = new GestorSolicitudesSalida();
            var solicitud = gestorSolicitud.ObtenerSolicitudSalida(idSolicitud);
            Mapper.CreateMap<DetalleSolicitudSalida, DetalleSolicitudSalidaDto>()
                .ForMember(d => d.NombreProducto, opts => opts.MapFrom(src => src.Producto.Nombre));
            Mapper.CreateMap<SolicitudSalida, SolicitudSalidaDto>()
                //.ForMember(s => s.TipoEntrada,
                //    opts => opts.MapFrom(src => Enums.GetEnumDescription((Enums.TipoEntrada)src.TipoEntrada)))
                .ForMember(s => s.NombreEmpleado,
                    opts => opts.MapFrom(src => src.Empleado.Nombres + ' ' + src.Empleado.Apellidos))
                .ForMember(s => s.Detalle,
                    opts => opts.MapFrom(src => src.DetalleSolicitudSalida));
            var dto = Mapper.Map<SolicitudSalidaDto>(solicitud);
            return dto;
        }

        public bool GenerarGuiaSalida(int idSolicitud, int idAlmacen, int idEmpleado)
        {
            var gestorGuia = new GestorGuiasSalida();
            return gestorGuia.GenerarGuiaSalida(idSolicitud, idAlmacen, idEmpleado);
        }

        public bool RechazarGuiaSalida(int idSolicitud, string observaciones)
        {
            var gestorGuia = new GestorGuiasSalida();
            return gestorGuia.RechazarGuiaSalida(idSolicitud, observaciones);
        }
        
        public List<EmpleadoDto> ObtenerEmpleados()
        {
            var gestorEmpleados = new GestorEmpleados();
            var lista = gestorEmpleados.ObtenerEmpleados();
            Mapper.CreateMap<Empleado, EmpleadoDto>();
            return Mapper.Map<List<EmpleadoDto>>(lista);
        }
    }
}
