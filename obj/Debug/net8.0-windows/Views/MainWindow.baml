   M S B A M L   `   `   `  ����  BiomentricoHolding#    BiomentricoHolding.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$!  Sistema Biométrico Holding��5   	    � WindowStartupLocation$ CenterScreen=�5   	   $	��600��5   	   $
��1000��6   ���#F5F7FA � 
ResizeMode$ CanResizeWithGrip=�5   	    � WindowStyle$
 None=�5	   	    � Icon$! /Assets/huella-dactilar.ico��5
   	    � AllowsTransparency$ False��5   	   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   �� -   5        $��0��5      ���#343a40._�5      ]� $
��10,0q�6   $��Center=�6&   .+�5      �� $*��$🛡️  Sistema Biométrico Holding��6   
�������$��16(�5      $
��Bold"�5      $��Center=�5      5      5      � 5   
   $��1��6    � ColumnDefinitions 5       �� 5!      $	��220 �6#   �� 5"      $��* �6#   .+�5&      �� ���#680307$��0��6.   .+�5'      �� $��Control de Ingreso��6   
�������$��18(�5)      $
��Bold"�5*      $��20,30,20,10q�5+      $��Center=�5,      �� 5-      $��VML Holding��6   
�������$��16(�5/      $
��Bold"�50      $��
10,0,20,10q�51      $��Center=�52      �� 53      $��10q�6   �� 56      -   5        $��📋 Registrar Empleado��56         SidebarButtonStyle#��[  58      �� 5:      -   5        $��📌 Entradas/Salidas��5:      #��[  5<      �� 5>      -   5        $��🗂 Consultar Registros��5>      #��[  5@      �� 5B      -   5        $��⚙ Configuración��5B      #��[  5D      �� 5F      -   5        $��🔒 Cerrar Sesión��5F      #��[  5H      $��10,40,10,10q�5I      �� 5K      -   5         ��Name$ 
lblVersion��5K      $��Versión 1.0.0��5L      
�������$��12(�5N      $��SemiBold"�5O      $��Center=�5P      $��
10,20,10,0q�5Q      �� 5S      -   5        $ lblFechaPublicacion��5S      $��Fecha de Publicación: ��5T      
�������$��12(�5V      $��SemiBold"�5W      $��Center=�5X      $��
10,0,10,10q�5Y      5Z      �� 5]      $��1��6   
�������	 �� Padding$	 20q�68   ._�5^      � .+�5`      �� -	   5        
 ��Name$
 imgBienvenida��5`      $��Uniform=�5a      $��Center=�5b      $��Center=�5c      $y�Visible=�5d      �� 5g      -
   5         ��Name$ MainContent��5g   %   $y�	Collapsed=�5h   %   5i      5j      5k      5l      5m      