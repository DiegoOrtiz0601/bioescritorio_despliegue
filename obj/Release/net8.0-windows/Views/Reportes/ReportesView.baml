   M S B A M L   `   `   `  ����   BiomentricoHolding4    .BiomentricoHolding.Views.Reportes.ReportesView   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ���	#FFF2F2F2  � 	Resources     BotonRojoRedondeado �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value��%�  &	        �� " ��.���  � �� �#dc3545��  � ��
 �������  � ��$ SemiBold"���  � ��$ 13(���  � ��$	 8,4q���  � ��$ 5,0,0,0q���  �@�Cursor$
 Hand����  � ��$ 0q���  � ��$ 35����  � ��$	 160����  � �� �� " ��.R��� $ 20��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �#212529.���� $
��Auto=�$
��Auto=�.��� $	��900��$	��500��.+��� ���#CCC$��1q�$ 10��
�������$��10q�._�� $��20q�	 � RowDefinitions	 �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+�]� ���#343a40$��40��.+��� $ ��📊 Generador de Reportes��
�������$
��Bold"�$��16(�$
��10,0q��� $��1��$��	0,20,0,10q�.+�� $��0,0,0,10q�.+��� -   
 ��Name$
 cbTipoReporte��$	��200��$��5q�.@��� ${�False�� �� 
IsSelected$
 True��.��Seleccione tipo de reporte�� .��Reporte por Empresa�� .��Reporte por Empleado(  "System.Windows.Controls.DatePicker  -     Name$ dpDesde��$	��170��$��5q�  -   $ dpHasta��$	��170��$��5q�� -    �Name$ panelEmpresa��$y�	Collapsed=�$��0,0,0,10q�.+��� -   $
 	cbEmpresa��$	��200��$��5q�.@��� ${�False��$
 True��.��Seleccione empresa�� -   $
 cbSede��$	��200��$��5q�.@��� ${�False��$
 True��.��Sede (opcional)�� -   $
 cbArea��$	��200��$��5q�.@��� ${�False��$
 True��.��Área (opcional)� -   $ panelEmpleado��$y�	Collapsed=�$��0,0,0,10q�.+�� $	��350��$��40��$��5q�.+��� -	    ��Name$ txtDocumento��$��Center=�$��10,0,0,0q�$��14(�
����������#ced4da$��1q��� -
    ��Name$ txtPlaceholder��$"��Ingrese número de documento��$��Italic$����#adb5bd$��12,0,0,0q�$��Center=� J� IsHitTestVisible$ False��$y�Visible=��� -   $��Generar Reporte��$��5q�#��[  &   System.Windows.Controls.DataGrid  -     Name$ 	dgReporte��$��2��   AutoGenerateColumns$ False��$��0,0,0,20q�   HeadersVisibility$ Column=�   CanUserAddRows$ False��   
IsReadOnly$
 True��   AlternatingRowBackground �#f1f3f5���#dee2e6$��1q�   RowHeaderWidth$ 0Q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 	Documento��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	Documento+  $ Nombre Completo��$	 150  �� *NombreCompleto+  $ Empresa��$	 120  �� *	Empresa+  $
 Sede��$	 100  �� *Sede+  $ Área��$	 100  �� *Area+  $
 Día��$	 100  �� *	DiaSemana+  $ Fecha��$	 100  �� *Fecha+  $ Entrada (Real)��$	 120  �� *HoraEntradaReal+  $ Salida (Real)��$	 120  �� *HoraSalidaReal+  $ 	Esperadas��$	 130  �� *HoraEsperada+  $ Retardo��$	 100  �� *	Retardo+  $ Estado��$ 60  �� *EstadoIcono+�� $��3�� �� Orientation$ 
Horizontal=�$��Right=�.+��� -   #��[  $��5q�.���� $ 
Horizontal=�$��Center=�.+��� $
��📁��$��16(�$��0,0,5,0q��� $��Exportar a Excel���� -   #��[  $��5q�.���� $ 
Horizontal=�$��Center=�.+��� $��✉️��$��16(�$��0,0,5,0q��� $��Enviar por Correo��