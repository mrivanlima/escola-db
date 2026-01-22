-- SETUP INICIAL (Rodar 1x)
create schema if not exists school;
create extension if not exists "uuid-ossp";

-- Função de Trigger para UpdatedAt
create or replace function public.handle_updated_at() 
returns trigger as $$
begin
  new.updated_at = now();
  return new;
end;
$$ language plpgsql;

-- EXEMPLO DE TABELA PADRÃO (STUDENTS)
create table school.students (
    -- 1. IDs Híbridos
    student_id      integer generated always as identity,
    student_uuid    uuid not null default gen_random_uuid(),
    tenant_id       integer not null,
    
    -- 2. Dados de Negócio
    nickname        text not null,
    birth_date      date not null,
    avatar_config   jsonb,
    
    -- 3. Auditoria Completa
    created_at      timestamptz not null default now(),
    created_by      integer not null, 
    updated_at      timestamptz,
    updated_by      integer,
    deleted_at      timestamptz, -- Soft Delete

    -- 4. Constraints Nomeadas (Bottom)
    constraint pk_students primary key (student_id),
    constraint uq_students_uuid unique (student_uuid),
    
    constraint fk_students_tenant foreign key (tenant_id) 
        references identity.tenants (tenant_id),
    
    constraint fk_students_created foreign key (created_by)
        references identity.app_users (user_id),

    constraint ck_students_nickname check (length(nickname) >= 2)
);

-- 5. Índices (Prefixo idx_)
create index idx_students_tenant_id on school.students(tenant_id);
create index idx_students_deleted_at on school.students(deleted_at) where deleted_at is null;

-- 6. Trigger Update
create trigger trg_students_updated_at
before update on school.students
for each row execute procedure public.handle_updated_at();

-- 7. RLS (Segurança)
alter table school.students enable row level security;

create policy "Tenant Isolation" on school.students
    using (tenant_id = current_setting('app.current_tenant')::integer);

-- 8. Metadata (Documentação)
COMMENT ON TABLE school.students IS 'Core entity: Child profile managed by parents/schools.';
COMMENT ON COLUMN school.students.student_id IS 'INTERNAL PK: Int. Never expose to API.';
COMMENT ON COLUMN school.students.avatar_config IS 'JSON: {"hair": int, "color": hex}. Visual customization.';