   M S B A M L   `   `   `  ����M  IBiomentricoHolding, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:BiomentricoHolding.Views.Configuracion&BiomentricoHolding.Views.Configuracion  5      A    ;BiomentricoHolding.Views.Configuracion.ConfiguracionControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Flocal4clr-namespace:BiomentricoHolding.Views.Configuracion     5      ���	#FFF2F2F2  � 	Resources  5         BotonRojoRedondeado �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value3  -System.Windows.Media.Effects.DropShadowEffect  
DropShadow	   Color
   	Direction   ShadowDepth   
BlurRadius   Opacity��%T  &	        &	 �    �� 5	   
   " ��6,   .�5
      ��  � �� �#dc35456+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 13(�6)   �� 5       � ��$	 8,4q�6(   �� 5       � ��$ 5,0,0,0q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ 0q�60   �� 5       � ��$ 35��6'   �� 5       � ��$	 130��6&   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 20��6!   #��z��63   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5       5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  � �� �#21252963   5       5"        5#   
   $	 Black��5$      $	
 270Q�5%      $ 4Q�5&      $ 12Q�5'      $	 0.4Q�5(      5)      .��5,      � .+�5/   
   �� ���#CCC$��1q�6$   $ 10��68   
�������$��10q�6]   ._�50      � $��20q�6    � RowDefinitions 51      �� 52      $
��Auto �6$   �� 53      $
��Auto �6$   �� 54      $��* �6$   �� 55      $
��Auto �6$   .+�59      ]� ���#343a40$��40��61   $	��Top=�6=   .+�5:      �� $$��⚙ Configuración del Sistema��6    
�������$
��Bold"�6V   $��16(�6h   $
��10,0q�6v   5;      �� 5>      $��1��6    �� Orientation$ 
Horizontal=�6*   $��	0,20,0,10q�6C   .+�5?      �� $��Sede actual:��6    $��SemiBold"�64   $��14(�6J   $��Center=�6X   $��0,0,10,0q�6s   �� 5@      -   5         ��Name$ cmbSedes��5@      $	��300��61   $��30��6=   $��14(�6I   �� 5A      -   5        #��[  5A      .��5B      �� $ 
Horizontal=�6%   .+�5C      �� $��Segoe MDL2 Assets)�6(   $	����6G   $��0,0,5,0q�6W   �� 5D      $��Guardar��6(   5E      5F      5G      �� 5J      $��2��6   $��Log del Sistema��6(   $��SemiBold"�6A   $��13(�6W   .��5K      �� $
��Auto=�6#   .��5L      �� -   5         ��Name$ txtLog��5L   "   $��Consolas)�5M   "   $��12(�5N   "    �� 
IsReadOnly$
 True��5O   "    �� TextWrapping$
 Wrap=�5P   "    �� AcceptsReturn$
 True��5Q   "   ���#f8f9fa$��0q�5S   "   $��10q�5T   "   5U      5V      �� 5Y      $��3��6   $ 
Horizontal=�6*   $��Right=�6C   $��0,10,0,0q�6_   .+�5Z      �� -   5        #��[  5Z      .��5[      �� $ 
Horizontal=�6%   .+�5\      �� $��Segoe MDL2 Assets)�6(   $	����6G   $��0,0,5,0q�6W   �� 5]      $��Descargar Log��6(   5^      5_      �� 5`      -   5        #��[  5`      .��5a      �� $ 
Horizontal=�6%   .+�5b      �� $��Segoe MDL2 Assets)�6(   $	����6G   $��0,0,5,0q�6W   �� 5c      $��Salir��6(   5d      5e      �� 5f      -   5        #��[  5f      .��5g      �� $ 
Horizontal=�6%   .+�5h      �� $��Segoe MDL2 Assets)�6(   $	����6G   $��0,0,5,0q�6W   �� 5i      $��Limpiar Log��6(   5j      5k      5l      5m      5n      � 5q   
   -   5         �Name$ ToastContainer��5q      $��Bottom=�5r      $��Center=�5s      $��0,0,0,20q�5t      $y�	Collapsed=�5u       K� ZIndex$
 1000��5v      .+�5w      �� ���#28a745$ 6��5x       �� Padding$
 10,6q�5y       J� Effect# �  5z      ._�5{      �� $ 
Horizontal=�6   .+�5|      �� $	��✔��6    $��16(�5}       $
��Bold"�5~       
�������$��0,0,6,0q�5�       �� 5�      -   5         ��Name$ ToastMessage��5�       $��14(�5�       
�������5�       5�      5�      5�      5�      5�      