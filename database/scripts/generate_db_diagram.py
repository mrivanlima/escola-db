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
    
    output_path = os.path.join('..', '..', 'docs', 'sqldocs', 'database_schema_diagram.pdf')
    
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
        ['Schemas', 'Tables', 'Normalized Columns', 'Extensions'],
        ['6', '17', '22', 'uuid-ossp, pgcrypto, unaccent']
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
                        'tenant_id (PK)', 'tenant_uuid (UK)', 'tenant_name', 
                        'tenant_name_normalized', 'tenant_type', 'is_active', 
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'app_users',
                    'columns': [
                        'user_id (PK)', 'user_uuid (UK)', 'tenant_id (FK)',
                        'auth_user_id (UK)', 'full_name', 'full_name_normalized',
                        'email', 'email_normalized', 'user_role', 'is_active',
                        '+ audit fields'
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
                        'file_id (PK)', 'tenant_id (FK)', 'storage_path',
                        'original_name', 'original_name_normalized', 'mime_type',
                        'size_bytes', 'alt_text', 'alt_text_normalized',
                        'metadata (JSONB)', '+ audit fields'
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
                        'user_id (FK)', 'phone_number', 'relationship',
                        'is_primary', '+ audit fields'
                    ]
                },
                {
                    'name': 'student_guardians',
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
                        'class_name + normalized', 'grade_level + normalized',
                        'school_year + normalized', 'max_students',
                        'class_config (JSONB)', 'is_active', '+ audit fields'
                    ]
                },
                {
                    'name': 'teachers',
                    'columns': [
                        'teacher_id (PK)', 'teacher_uuid (UK)', 'tenant_id (FK)',
                        'user_id (FK)', 'specialization + normalized',
                        'hire_date', 'teacher_config (JSONB)', 'is_active',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'class_students',
                    'columns': [
                        'class_student_id (PK)', 'class_id (FK)',
                        'student_id (FK)', 'tenant_id (FK)',
                        'enrollment_date', 'status', '+ audit fields'
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
                        'module_type', 'difficulty_level',
                        'recommended_age_min/max', 'display_order',
                        'thumbnail_url', 'module_config (JSONB)',
                        'is_published', '+ audit fields'
                    ]
                },
                {
                    'name': 'activities',
                    'columns': [
                        'activity_id (PK)', 'activity_uuid (UK)',
                        'module_id (FK)', 'activity_name + normalized',
                        'description + normalized', 'activity_type',
                        'display_order', 'estimated_duration', 'points_reward',
                        'activity_data (JSONB)', 'thumbnail_url',
                        'is_published', '+ audit fields'
                    ]
                },
                {
                    'name': 'activity_resources',
                    'columns': [
                        'resource_id (PK)', 'resource_uuid (UK)',
                        'activity_id (FK)', 'resource_name + normalized',
                        'resource_type', 'media_file_id (FK UUID)',
                        'display_order', 'is_required', 'usage_context',
                        'resource_config (JSONB)', 'is_published',
                        '+ audit fields'
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
                        'status', 'score', 'attempts', 'time_spent_seconds',
                        'progress_data (JSONB)', 'completed_at',
                        '+ audit fields'
                    ]
                },
                {
                    'name': 'badges',
                    'columns': [
                        'badge_id (PK)', 'badge_uuid (UK)',
                        'badge_name + normalized', 'description + normalized',
                        'badge_type', 'icon_url', 'rarity', 'points_value',
                        'unlock_criteria (JSONB)', 'display_order',
                        'is_active', '+ audit fields'
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
        ['tenants', 'app_users', '1:N', 'Multi-tenant isolation'],
        ['tenants', 'students', '1:N', 'School students'],
        ['tenants', 'media_files', '1:N', 'Tenant media library'],
        ['app_users', 'guardians', '1:1', 'User is guardian'],
        ['app_users', 'teachers', '1:1', 'User is teacher'],
        ['students', 'student_guardians', '1:N', 'Student has guardians'],
        ['guardians', 'student_guardians', '1:N', 'Guardian has students'],
        ['students', 'class_students', '1:N', 'Student enrollment'],
        ['classes', 'class_students', '1:N', 'Class roster'],
        ['modules', 'activities', '1:N', 'Module contains activities'],
        ['activities', 'activity_resources', '1:N', 'Activity uses resources'],
        ['media_files', 'activity_resources', '1:N', 'Media library reference'],
        ['students', 'student_progress', '1:N', 'Progress tracking'],
        ['activities', 'student_progress', '1:N', 'Activity completion'],
        ['students', 'student_badges', '1:N', 'Earned badges'],
        ['badges', 'student_badges', '1:N', 'Badge awards']
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
        ['Text Normalization', '22 *_normalized columns for accent-insensitive search (José → jose)'],
        ['Multi-Tenancy', 'RLS enabled for tenant isolation via tenant_id'],
        ['Audit Trail', 'All tables: created_at/by, updated_at/by, deleted_at (soft delete)'],
        ['JSONB Columns', 'Flexible config/metadata storage (avatar_config, module_config, etc.)'],
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
    print(f"   - 17 tables across 6 schemas")
    print(f"   - 22 normalized columns for accent-insensitive search")
    print(f"   - Complete relationship diagram included")

if __name__ == '__main__':
    create_database_diagram_pdf()
