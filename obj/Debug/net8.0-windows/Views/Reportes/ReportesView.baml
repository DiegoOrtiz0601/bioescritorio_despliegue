   M S B A M L   `   `   `  ����  BiomentricoHolding4    .BiomentricoHolding.Views.Reportes.ReportesView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ���	#FFF2F2F2  � 	Resources  5         BotonRojoRedondeado �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value��%�  &	        �� 5	   
   " ��6,   .�5
      ��  � �� �#dc35456+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 13(�6)   �� 5       � ��$	 8,4q�6(   �� 5       � ��$ 5,0,0,0q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ 0q�60   �� 5       � ��$ 35��6'   �� 5       � ��$	 160��6&   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 20��6!   #��z��63   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5       5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  � �� �#21252963   5       5"      5#      .��5%      �� $
��Auto=�6   $
��Auto=�66   .��5&   
   � $	��900��6   $	��500��6   .+�5'      �� ���#CCC$��1q�6(   $ 10��6<   
�������$��10q�6a   ._�5(      � $��20q�6   	 � RowDefinitions	 5)      �� 5*      $
��Auto �6(   �� 5+      $
��Auto �6(   �� 5,      $��* �6(   �� 5-      $
��Auto �6(   .+�51      ]� ���#343a40$��40��65   .+�52      �� $ ��📊 Generador de Reportes��6$   
�������$
��Bold"�54   $   $��16(�55   $   $
��10,0q�56   $   57      �� 5:      $��1��6!   $��	0,20,0,10q�6.   .+�5;      � $��0,0,0,10q�6$   .+�5<      �� -   5        
 ��Name$
 cbTipoReporte��5<   '   $	��200��6>   $��5q�6J   .@�5=   "   �� ${�False��6/    �� 
IsSelected$
 True��6A   .��6S   Seleccione tipo de reporte6o   �� 5>   "   .��6/   Reporte por Empresa6D   �� 5?   "   .��6/   Reporte por Empleado6E   5@      (  "System.Windows.Controls.DatePicker  5A      -   5          Name$ dpDesde��5A   )   $	��170��6:   $��5q�6F     5B      -   5        $ dpHasta��5B   )   $	��170��6:   $��5q�6F   5C      � 5E      -   5         �Name$ panelEmpresa��5E   $   $y�	Collapsed=�6:   $��0,0,0,10q�6Q   .+�5F      �� -   5        $
 	cbEmpresa��5F   '   $	��200��6:   $��5q�6F   .@�5G   "   �� ${�False��6/   $
 True��6A   .��6S   Seleccione empresa6g   5H      �� 5I      -   5        $
 cbSede��5I   '   $	��200��67   $��5q�6C   .@�5J   "   �� ${�False��6/   $
 True��6A   .��6S   Sede (opcional)6d   5K      �� 5L      -   5        $
 cbArea��5L   '   $	��200��67   $��5q�6C   .@�5M   "   �� ${�False��6/   $
 True��6A   .��6S   Área (opcional)6d   5N      5O      � 5Q      -   5        $ panelEmpleado��5Q   $   $y�	Collapsed=�6;   $��0,0,0,10q�6R   .+�5R      � $	��350��6#   $��40��6/   $��5q�6;   .+�5S   "   �� -	   5         ��Name$ txtDocumento��5S   *   $��Center=�5T   *   $��10,0,0,0q�5U   *   $��14(�5V   *   
����������#ced4da$��1q�5Y   *   �� 5Z   "   -
   5         ��Name$ txtPlaceholder��5Z   ,   $"��Ingrese número de documento��5[   ,   $��Italic$�5\   ,   ���#adb5bd$��12,0,0,0q�5^   ,   $��Center=�5_   ,    J� IsHitTestVisible$ False��5`   ,   $y�Visible=�5a   ,   5b      5c      �� 5e      -   5        $��Generar Reporte��5e   !   $��5q�5f   !   #��[  5g   !   5h   !   5i      &   System.Windows.Controls.DataGrid  5l      -   5          Name$ 	dgReporte��5l      $��2��5m         AutoGenerateColumns$ False��5n      $��0,0,0,20q�5o         HeadersVisibility$ Column=�5p         CanUserAddRows$ False��5q         
IsReadOnly$
 True��5r         AlternatingRowBackground �#f1f3f5���#dee2e6$��1q�5u         RowHeaderWidth$ 0Q�5v         Columns 5w      0  *System.Windows.Controls.DataGridTextColumn  5x      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Documento��61      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6b   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *	Documento+6b     5y      $ Nombre Completo��61   $	 150 6m    6J   �� *NombreCompleto+6m     5z      $ Empresa��61   $	 120 6^    6B   �� *	Empresa+6^     5{      $
 Sede��61   $	 100 6X    6?   �� *Sede+6X     5|      $ Área��61   $	 100 6X    6?   �� *Area+6X     5}      $
 Día��61   $	 100 6\    6>   �� *	DiaSemana+6\     5~      $ Fecha��61   $	 100 6Z    6@   �� *Fecha+6Z     5      $ Entrada (Real)��61   $	 120 6m    6I   �� *HoraEntradaReal+6m     5�      $ Salida (Real)��61   $	 120 6k    6H   �� *HoraSalidaReal+6k     5�      $ 	Esperadas��61   $	 130 6e    6D   �� *HoraEsperada+6e     5�      $ Retardo��61   $	 100 6^    6B   �� *	Retardo+6^     5�      $ Estado��61   $ 60 6a    6A   �� *EstadoIcono+6a   5�      �� 5�      $��3��6!    �� Orientation$ 
Horizontal=�6.   $��Right=�6G   .+�5�      �� -   5        #��[  5�   !   $��5q�6N   .��5�      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5�   "   �� $
��📁��6,   $��16(�66   $��0,0,5,0q�6D   �� 5�   "   $��Exportar a Excel��6,   5�      5�      �� 5�      -   5        #��[  5�   !   $��5q�6N   .��5�      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5�   "   �� $��✉️��6,   $��16(�66   $��0,0,5,0q�6D   �� 5�   "   $��Enviar por Correo��6,   5�      5�      5�      5�      5�      5�      5�      5�      