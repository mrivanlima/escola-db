"""
Database Schema Diagram Generator
Generates a PDF visualization of the Escola Platform database schema
"""

from reportlab.lib import colors
from reportlab.lib.pagesizes import A3, landscape
from reportlab.platypus import SimpleDocTemplate, Table, TableStyle, Paragraph, Spacer, PageBreak
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import inch
from reportlab.lib.enums import TA_CENTER, TA_LEFT
import os

def create_database_diagram_pdf():
    """Generate PDF with database schema tables and relationships"""
    
    output_path = os.path.join('docs', 'sqldocs', 'database_schema_diagram.pdf')
    
    # Create PDF document
    doc = SimpleDocTemplate(
        output_path,
        pagesize=landscape(A3),
        rightMargin=30,
        leftMargin=30,
        topMargin=50,
        bottomMargin=30
    )
    
    # Container for elements
    elements = []
    
    # Styles
    styles = getSampleStyleSheet()
    title_style = ParagraphStyle(
        'CustomTitle',
        parent=styles['Heading1'],
        fontSize=24,
        textColor=colors.HexColor('#1a237e'),
        spaceAfter=30,
        alignment=TA_CENTER
    )
    
    heading_style = ParagraphStyle(
        'CustomHeading',
        parent=styles['Heading2'],
        fontSize=16,
        textColor=colors.HexColor('#283593'),
        spaceAfter=12,
        spaceBefore=20
    )
    
    # Title
    elements.append(Paragraph("Escola Platform - Database Schema", title_style))
    elements.append(Spacer(1, 0.2*inch))
    
    # Summary
    summary_data = [
        ['Schemas', 'Tables', 'Lookup Tables', 'Extensions'],
        ['6', '41', '19', 'uuid-ossp, pgcrypto, unaccent']
    ]
    summary_table = Table(summary_data, colWidths=[2*inch, 2*inch, 2.5*inch, 3*inch])
    summary_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a237e')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'CENTER'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, 0), 12),
        ('BOTTOMPADDING', (0, 0), (-1, 0), 12),
        ('BACKGROUND', (0, 1), (-1, -1), colors.HexColor('#e8eaf6')),
        ('GRID', (0, 0), (-1, -1), 1, colors.black)
    ]))
    elements.append(summary_table)
    elements.append(Spacer(1, 0.3*inch))
    
    # Schema definitions
    schemas = [
        {
            'name': 'IDENTITY SCHEMA',
            'color': '#E8F4F8',
            'tables': [
                {
                    'name': 'tenants',
                    'columns': [
                        'tenant_id (PK)', 'tenant_uuid (UK)', 'tenant_type_id (FK)',
                        'tenant_name', 'tenant_name_normalized', 'tenant_config (JSONB)',
                        'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'tenant_types [LOOKUP]',
                    'columns': [
                        'type_id (PK)', 'type_uuid (UK)', 'tenant_id (FK)', 'type_code (UK)',
                        'type_name', 'description', 'max_students', 'max_teachers',
                        'max_storage_gb', 'has_analytics', 'has_api_access',
                        'has_white_label', 'pricing_tier', 'icon_name', 'color_code',
                        'display_order'
                    ]
                },
                {
                    'name': 'app_users',
                    'columns': [
                        'user_id (PK)', 'user_uuid (UK)', 'tenant_id (FK)',
                        'auth_user_id (UK)', 'full_name', 'full_name_normalized',
                        'email', 'email_normalized', 'user_role_id (FK)',
                        'is_active', 'user_config (JSONB)', '+ audit fields'
                    ]
                },
                {
                    'name': 'user_roles [LOOKUP]',
                    'columns': [
                        'role_id (PK)', 'role_uuid (UK)', 'tenant_id (FK)', 'role_code (UK)',
                        'role_name', 'description', 'permission_level',
                        'can_manage_students', 'can_manage_content',
                        'can_manage_users', 'can_manage_tenant',
                        'icon_name', 'color_code', 'display_order'
                    ]
                }
            ]
        },
        {
            'name': 'ASSETS SCHEMA',
            'color': '#FFF4E6',
            'tables': [
                {
                    'name': 'media_files',
                    'columns': [
                        'file_id (PK)', 'file_uuid (UK)', 'tenant_id (FK)',
                        'mime_type_id (FK)', 'storage_path', 'original_name',
                        'original_name_normalized', 'size_bytes', 'alt_text',
                        'alt_text_normalized', 'metadata (JSONB)', '+ audit fields'
                    ]
                },
                {
                    'name': 'mime_types [LOOKUP]',
                    'columns': [
                        'mime_type_id (PK)', 'mime_type_uuid (UK)', 'tenant_id (FK)',
                        'category_id (FK)', 'mime_type_code (UK)', 'mime_type_name',
                        'file_extension', 'max_file_size_mb', 'icon_name', 'display_order'
                    ]
                },
                {
                    'name': 'media_categories [LOOKUP]',
                    'columns': [
                        'category_id (PK)', 'category_uuid (UK)', 'tenant_id (FK)',
                        'category_code (UK)', 'category_name', 'description',
                        'default_icon_name', 'default_max_size_mb', 'display_order'
                    ]
                }
            ]
        },
        {
            'name': 'SCHOOL SCHEMA',
            'color': '#E8F5E9',
            'tables': [
                {
                    'name': 'students',
                    'columns': [
                        'student_id (PK)', 'student_uuid (UK)', 'tenant_id (FK)',
                        'user_id (FK, Nullable - Hybrid Auth)',
                        'nickname + normalized', 'first_name + normalized',
                        'middle_name + normalized', 'last_name + normalized',
                        'birth_date', 'avatar_config (JSONB)', 'is_active',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'guardians',
                    'columns': [
                        'guardian_id (PK)', 'guardian_uuid (UK)', 'tenant_id (FK)',
                        'user_id (FK)', 'phone_number', 'relationship_type_id (FK)',
                        'is_primary', '+ audit fields'
                    ]
                },
                {
                    'name': 'relationship_types [LOOKUP]',
                    'columns': [
                        'relationship_type_id (PK)', 'relationship_uuid (UK)',
                        'tenant_id (FK)', 'relationship_code (UK)', 'relationship_name',
                        'description', 'can_authorize', 'requires_legal_proof',
                        'icon_name', 'display_order'
                    ]
                },
                {
                    'name': 'student_guardians [JUNCTION]',
                    'columns': [
                        'student_guardian_id (PK)', 'student_id (FK)',
                        'guardian_id (FK)', 'tenant_id (FK)',
                        'relationship_notes + normalized',
                        'is_authorized_pickup', '+ audit fields'
                    ]
                },
                {
                    'name': 'classes',
                    'columns': [
                        'class_id (PK)', 'class_uuid (UK)', 'tenant_id (FK)',
                        'class_name + normalized', 'grade_level_id (FK)',
                        'school_year_id (FK)', 'max_students',
                        'class_config (JSONB)', 'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'grade_levels [LOOKUP]',
                    'columns': [
                        'grade_level_id (PK)', 'grade_uuid (UK)', 'tenant_id (FK)',
                        'grade_code (UK)', 'grade_name', 'description',
                        'age_range_min', 'age_range_max', 'display_order',
                        'icon_name', 'color_code'
                    ]
                },
                {
                    'name': 'school_years [LOOKUP]',
                    'columns': [
                        'school_year_id (PK)', 'year_uuid (UK)', 'tenant_id (FK)',
                        'year_code (UK)', 'year_name', 'description',
                        'start_date', 'end_date', 'is_current', 'display_order'
                    ]
                },
                {
                    'name': 'teachers',
                    'columns': [
                        'teacher_id (PK)', 'teacher_uuid (UK)', 'tenant_id (FK)',
                        'user_id (FK)', 'specialization_id (FK)',
                        'hire_date', 'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'specializations [LOOKUP]',
                    'columns': [
                        'specialization_id (PK)', 'specialization_uuid (UK)',
                        'specialization_name (UK)', 'description', 'is_active',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'certifications [LOOKUP]',
                    'columns': [
                        'certification_id (PK)', 'certification_uuid (UK)',
                        'certification_name (UK)', 'description',
                        'issuing_organization', 'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'teacher_certifications [JUNCTION]',
                    'columns': [
                        'teacher_id (FK)', 'certification_id (FK)',
                        'obtained_date', 'expiry_date', 'credential_number',
                        'notes', '+ audit fields'
                    ]
                },
                {
                    'name': 'subjects [LOOKUP]',
                    'columns': [
                        'subject_id (PK)', 'subject_uuid (UK)', 'subject_name (UK)',
                        'description', 'grade_level_id (FK)', 'is_active',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'teacher_subjects [JUNCTION]',
                    'columns': [
                        'teacher_id (FK)', 'subject_id (FK)',
                        'proficiency_level_id (FK)', 'years_experience',
                        'is_primary_subject', 'notes', '+ audit fields'
                    ]
                },
                {
                    'name': 'proficiency_levels [LOOKUP]',
                    'columns': [
                        'proficiency_level_id (PK)', 'proficiency_uuid (UK)',
                        'tenant_id (FK)', 'proficiency_code (UK)', 'proficiency_name',
                        'description', 'minimum_years', 'icon_name', 'color_code',
                        'display_order'
                    ]
                },
                {
                    'name': 'class_students [JUNCTION]',
                    'columns': [
                        'class_student_id (PK)', 'class_id (FK)',
                        'student_id (FK)', 'tenant_id (FK)',
                        'enrollment_date', 'status_id (FK)', '+ audit fields'
                    ]
                },
                {
                    'name': 'enrollment_statuses [LOOKUP]',
                    'columns': [
                        'status_id (PK)', 'status_uuid (UK)', 'tenant_id (FK)',
                        'status_code (UK)', 'status_name', 'description',
                        'allows_attendance', 'allows_grading', 'is_final_state',
                        'icon_name', 'color_code', 'display_order'
                    ]
                }
            ]
        },
        {
            'name': 'CONTENT SCHEMA',
            'color': '#F3E5F5',
            'tables': [
                {
                    'name': 'modules',
                    'columns': [
                        'module_id (PK)', 'module_uuid (UK)',
                        'module_name + normalized', 'description + normalized',
                        'module_type_id (FK)', 'difficulty_level',
                        'recommended_age_min/max', 'display_order',
                        'thumbnail_url', 'module_config (JSONB)',
                        'is_published', '+ audit fields'
                    ]
                },
                {
                    'name': 'module_types [LOOKUP]',
                    'columns': [
                        'module_type_id (PK)', 'module_type_uuid (UK)',
                        'tenant_id (FK)', 'module_type_code (UK)', 'module_type_name',
                        'description', 'icon_name', 'color_code', 'display_order'
                    ]
                },
                {
                    'name': 'activities',
                    'columns': [
                        'activity_id (PK)', 'activity_uuid (UK)',
                        'module_id (FK)', 'activity_name + normalized',
                        'description + normalized', 'activity_type_id (FK)',
                        'display_order', 'estimated_duration', 'points_reward',
                        'activity_data (JSONB)', 'thumbnail_url',
                        'is_published', '+ audit fields'
                    ]
                },
                {
                    'name': 'activity_types [LOOKUP]',
                    'columns': [
                        'activity_type_id (PK)', 'activity_type_uuid (UK)',
                        'tenant_id (FK)', 'activity_type_code (UK)', 'activity_type_name',
                        'description', 'icon_name', 'default_points',
                        'requires_interaction', 'display_order'
                    ]
                },
                {
                    'name': 'activity_resources',
                    'columns': [
                        'resource_id (PK)', 'resource_uuid (UK)',
                        'activity_id (FK)', 'resource_name + normalized',
                        'resource_type_id (FK)', 'media_file_id (FK)',
                        'display_order', 'is_required', 'usage_context_id (FK)',
                        'resource_config (JSONB)', 'is_published',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'resource_types [LOOKUP]',
                    'columns': [
                        'resource_type_id (PK)', 'resource_type_uuid (UK)',
                        'tenant_id (FK)', 'resource_type_code (UK)', 'resource_type_name',
                        'description', 'mime_types (JSONB)', 'max_file_size_mb',
                        'icon_name', 'display_order'
                    ]
                },
                {
                    'name': 'usage_contexts [LOOKUP]',
                    'columns': [
                        'usage_context_id (PK)', 'usage_context_uuid (UK)',
                        'tenant_id (FK)', 'context_code (UK)', 'context_name',
                        'description', 'icon_name', 'display_order'
                    ]
                }
            ]
        },
        {
            'name': 'GAME SCHEMA',
            'color': '#FFEBEE',
            'tables': [
                {
                    'name': 'student_progress',
                    'columns': [
                        'progress_id (PK)', 'progress_uuid (UK)',
                        'student_id (FK)', 'activity_id (FK)', 'tenant_id (FK)',
                        'status_id (FK)', 'score', 'attempts_count',
                        'time_spent_seconds', 'progress_data (JSONB)',
                        'completion_date', '+ audit fields'
                    ]
                },
                {
                    'name': 'progress_statuses [LOOKUP]',
                    'columns': [
                        'status_id (PK)', 'status_uuid (UK)', 'tenant_id (FK)',
                        'status_code (UK)', 'status_name', 'description',
                        'icon_name', 'color_code', 'is_final_state', 'display_order'
                    ]
                },
                {
                    'name': 'badges',
                    'columns': [
                        'badge_id (PK)', 'badge_uuid (UK)',
                        'badge_name + normalized', 'description + normalized',
                        'badge_type_id (FK)', 'icon_url', 'rarity_id (FK)',
                        'points_value', 'unlock_criteria (JSONB)', 'display_order',
                        'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'badge_types [LOOKUP]',
                    'columns': [
                        'badge_type_id (PK)', 'badge_type_uuid (UK)',
                        'badge_type_code (UK)', 'badge_type_name', 'description',
                        'icon_name', 'display_order', 'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'badge_rarities [LOOKUP]',
                    'columns': [
                        'rarity_id (PK)', 'rarity_uuid (UK)', 'rarity_code (UK)',
                        'rarity_name', 'description', 'color_code',
                        'points_multiplier', 'display_order', 'is_active',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'student_badges',
                    'columns': [
                        'student_badge_id (PK)', 'student_id (FK)',
                        'badge_id (FK)', 'tenant_id (FK)', 'earned_at',
                        'earn_metadata (JSONB)', '+ audit fields'
                    ]
                }
            ]
        }
    ]
    
    # Generate tables for each schema
    for schema in schemas:
        elements.append(Paragraph(schema['name'], heading_style))
        
        for table_def in schema['tables']:
            # Create table data
            table_data = [['Table: ' + table_def['name']]]
            for col in table_def['columns']:
                table_data.append([col])
            
            # Create table
            t = Table(table_data, colWidths=[4*inch])
            t.setStyle(TableStyle([
                ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#283593')),
                ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
                ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
                ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
                ('FONTSIZE', (0, 0), (-1, 0), 11),
                ('FONTSIZE', (0, 1), (-1, -1), 9),
                ('BOTTOMPADDING', (0, 0), (-1, 0), 10),
                ('TOPPADDING', (0, 1), (-1, -1), 4),
                ('BOTTOMPADDING', (0, 1), (-1, -1), 4),
                ('BACKGROUND', (0, 1), (-1, -1), colors.HexColor(schema['color'])),
                ('GRID', (0, 0), (-1, -1), 0.5, colors.grey),
                ('VALIGN', (0, 0), (-1, -1), 'TOP')
            ]))
            elements.append(t)
            elements.append(Spacer(1, 0.15*inch))
    
    # Add relationships page
    elements.append(PageBreak())
    elements.append(Paragraph("Key Relationships", title_style))
    elements.append(Spacer(1, 0.2*inch))
    
    relationships_data = [
        ['From', 'To', 'Type', 'Description'],
        ['tenants', 'tenant_types', 'N:1', 'Tenant classification (school/enterprise)'],
        ['tenants', 'app_users', '1:N', 'Multi-tenant user isolation'],
        ['app_users', 'user_roles', 'N:1', 'User role with permissions'],
        ['tenants', 'students', '1:N', 'School students'],
        ['app_users', 'students', '0..1:1', 'Optional student login (Hybrid Auth Model)'],
        ['tenants', 'media_files', '1:N', 'Tenant media library'],
        ['media_files', 'mime_types', 'N:1', 'File type classification'],
        ['mime_types', 'media_categories', 'N:1', 'MIME category'],
        ['app_users', 'guardians', '1:1', 'User is guardian'],
        ['app_users', 'teachers', '1:1', 'User is teacher'],
        ['guardians', 'relationship_types', 'N:1', 'Guardian relationship type'],
        ['students', 'student_guardians', '1:N', 'Student has guardians'],
        ['guardians', 'student_guardians', '1:N', 'Guardian has students'],
        ['students', 'class_students', '1:N', 'Student enrollment'],
        ['classes', 'class_students', '1:N', 'Class roster'],
        ['class_students', 'enrollment_statuses', 'N:1', 'Enrollment status'],
        ['classes', 'grade_levels', 'N:1', 'Class grade level'],
        ['classes', 'school_years', 'N:1', 'Academic year'],
        ['teachers', 'specializations', 'N:1', 'Teacher specialization'],
        ['teachers', 'teacher_certifications', '1:N', 'Teacher certifications'],
        ['certifications', 'teacher_certifications', '1:N', 'Certification type'],
        ['teachers', 'teacher_subjects', '1:N', 'Subjects taught'],
        ['subjects', 'teacher_subjects', '1:N', 'Teachers for subject'],
        ['teacher_subjects', 'proficiency_levels', 'N:1', 'Teaching proficiency'],
        ['subjects', 'grade_levels', 'N:1', 'Subject grade level'],
        ['modules', 'module_types', 'N:1', 'Module classification'],
        ['modules', 'activities', '1:N', 'Module contains activities'],
        ['activities', 'activity_types', 'N:1', 'Activity classification'],
        ['activities', 'activity_resources', '1:N', 'Activity uses resources'],
        ['activity_resources', 'resource_types', 'N:1', 'Resource classification'],
        ['activity_resources', 'usage_contexts', 'N:1', 'Resource usage context'],
        ['activity_resources', 'media_files', 'N:1', 'Media library reference'],
        ['students', 'student_progress', '1:N', 'Progress tracking'],
        ['activities', 'student_progress', '1:N', 'Activity completion'],
        ['student_progress', 'progress_statuses', 'N:1', 'Progress status'],
        ['students', 'student_badges', '1:N', 'Earned badges'],
        ['badges', 'student_badges', '1:N', 'Badge awards'],
        ['badges', 'badge_types', 'N:1', 'Badge classification'],
        ['badges', 'badge_rarities', 'N:1', 'Badge rarity level']
    ]
    
    rel_table = Table(relationships_data, colWidths=[2*inch, 2*inch, 1*inch, 4*inch])
    rel_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a237e')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('ALIGN', (2, 0), (2, -1), 'CENTER'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, 0), 11),
        ('FONTSIZE', (0, 1), (-1, -1), 9),
        ('BOTTOMPADDING', (0, 0), (-1, 0), 12),
        ('TOPPADDING', (0, 1), (-1, -1), 6),
        ('BOTTOMPADDING', (0, 1), (-1, -1), 6),
        ('BACKGROUND', (0, 1), (-1, -1), colors.HexColor('#f5f5f5')),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f5f5f5')]),
        ('GRID', (0, 0), (-1, -1), 0.5, colors.grey)
    ]))
    elements.append(rel_table)
    
    # Add key features
    elements.append(Spacer(1, 0.3*inch))
    elements.append(Paragraph("Key Features", heading_style))
    
    features_data = [
        ['Feature', 'Description'],
        ['Hybrid ID Strategy', 'Internal INT (student_id) + External UUID (student_uuid)'],
        ['3NF Normalization', 'All free-text enums converted to lookup tables with metadata'],
        ['Text Normalization', '30+ *_normalized columns for accent-insensitive search (José → jose)'],
        ['Multi-Tenancy', 'RLS enabled for tenant isolation via tenant_id'],
        ['Audit Trail', 'All tables: created_at/by, updated_at/by, deleted_at (soft delete)'],
        ['JSONB Validation', 'CHECK constraints ensure data integrity for all JSONB columns'],
        ['Lookup Tables', '19 lookup tables with icons, colors, metadata for rich UI'],
        ['Extensions', 'uuid-ossp (UUID gen), pgcrypto (encryption), unaccent (search)']
    ]
    
    features_table = Table(features_data, colWidths=[2.5*inch, 6.5*inch])
    features_table.setStyle(TableStyle([
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#1a237e')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, -1), 'LEFT'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, 0), 11),
        ('FONTSIZE', (0, 1), (-1, -1), 9),
        ('BOTTOMPADDING', (0, 0), (-1, 0), 12),
        ('TOPPADDING', (0, 1), (-1, -1), 6),
        ('BOTTOMPADDING', (0, 1), (-1, -1), 6),
        ('ROWBACKGROUNDS', (0, 1), (-1, -1), [colors.white, colors.HexColor('#f5f5f5')]),
        ('GRID', (0, 0), (-1, -1), 0.5, colors.grey),
        ('VALIGN', (0, 0), (-1, -1), 'TOP')
    ]))
    elements.append(features_table)
    
    # Build PDF
    doc.build(elements)
    
    print(f"✅ PDF created successfully: {output_path}")
    print(f"   - 41 tables across 6 schemas")
    print(f"   - 19 lookup tables with metadata")
    print(f"   - 4 junction tables for many-to-many relationships")
    print(f"   - 30+ normalized columns for accent-insensitive search")
    print(f"   - Complete 3NF normalization with all enums as FKs")
    print(f"   - Complete relationship diagram included")

if __name__ == '__main__':
    create_database_diagram_pdf()
