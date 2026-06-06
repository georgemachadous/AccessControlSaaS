using AutoMapper;
using AccessControl.Domain.Entities;
using AccessControl.Application.DTOs;

namespace AccessControl.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Empresa, EmpresaDto>();
        CreateMap<CriarEmpresaDto, Empresa>();
        CreateMap<AtualizarEmpresaDto, Empresa>();

        CreateMap<Filial, FilialDto>();
        CreateMap<CriarFilialDto, Filial>();
        CreateMap<AtualizarFilialDto, Filial>();

        CreateMap<Usuario, UsuarioDto>();
        CreateMap<CriarUsuarioDto, Usuario>();
        CreateMap<AtualizarUsuarioDto, Usuario>();

        CreateMap<Perfil, PerfilDto>();
        CreateMap<CriarPerfilDto, Perfil>();
        CreateMap<AtualizarPerfilDto, Perfil>();

        CreateMap<Funcionalidade, FuncionalidadeDto>();
        CreateMap<CriarFuncionalidadeDto, Funcionalidade>();
        CreateMap<AtualizarFuncionalidadeDto, Funcionalidade>();

        CreateMap<Aplicacao, AplicacaoDto>();
        CreateMap<CriarAplicacaoDto, Aplicacao>();
        CreateMap<AtualizarAplicacaoDto, Aplicacao>();

        CreateMap<Notificacao, NotificacaoDto>();
        CreateMap<CriarNotificacaoDto, Notificacao>();

        CreateMap<PerfilFuncionalidade, PerfilFuncionalidadeDto>();
        CreateMap<UsuarioPerfilAplicacao, UsuarioPerfilAplicacaoDto>();
    }
}
