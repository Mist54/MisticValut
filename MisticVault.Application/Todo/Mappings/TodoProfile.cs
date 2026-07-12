using AutoMapper;
using MisticVault.Application.Todo.DTOs.Todo;
using MisticVault.Core.Todo.Entities;

namespace MisticVault.Application.Todo.Mappings
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<CreateTodoRequestDTO, MisticVault.Core.Todo.Entities.Todo>();
            CreateMap<UpdateTodoRequestDTO, MisticVault.Core.Todo.Entities.Todo>();
            CreateMap<MisticVault.Core.Todo.Entities.Todo, TodoResponseDTO>();
            CreateMap<MisticVault.Core.Todo.Entities.TodoCategory, TodoCategoryResponseDTO>();
        }
    }
}
