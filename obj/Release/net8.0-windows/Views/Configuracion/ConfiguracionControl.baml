   M S B A M L   `   `   `  ���� M  IBiomentricoHolding, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:BiomentricoHolding.Views.Configuracion&BiomentricoHolding.Views.Configuracion  A    ;BiomentricoHolding.Views.Configuracion.ConfiguracionControl   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Flocal4clr-namespace:BiomentricoHolding.Views.Configuracion     ���	#FFF2F2F2  � 	Resources     BotonRojoRedondeado �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value3  -System.Windows.Media.Effects.DropShadowEffect  
DropShadow	   Color
   	Direction   ShadowDepth   
BlurRadius   Opacity��%�  &	        &	 �    �� " ��.���  � �� �#dc3545��  � ��
 �������  � ��$ SemiBold"���  � ��$ 13(���  � ��$	 8,4q���  � ��$ 5,0,0,0q���  �@�Cursor$
 Hand����  � ��$ 0q���  � ��$ 35����  � ��$	 130����  � �� �� " ��.R��� $ 20��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �#212529  $	 Black��$	
 270Q�$ 4Q�$ 12Q�$	 0.4Q�.��� .+��� ���#CCC$��1q�$ 10��
�������$��10q�._�� $��20q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+�]� ���#343a40$��40��$	��Top=�.+��� $$��⚙ Configuración del Sistema��
�������$
��Bold"�$��16(�$
��10,0q��� $��1�� �� Orientation$ 
Horizontal=�$��	0,20,0,10q�.+��� $��Sede actual:��$��SemiBold"�$��14(�$��Center=�$��0,0,10,0q��� -    ��Name$ cmbSedes��$	��300��$��30��$��14(��� -   #��[  .���� $ 
Horizontal=�.+��� $��Segoe MDL2 Assets)�$	����$��0,0,5,0q��� $��Guardar���� $��2��$��Log del Sistema��$��SemiBold"�$��13(�.���� $
��Auto=�.���� -    ��Name$ txtLog��$��Consolas)�$��12(� �� 
IsReadOnly$
 True�� �� TextWrapping$
 Wrap=� �� AcceptsReturn$
 True�����#f8f9fa$��0q�$��10q��� $��3��$ 
Horizontal=�$��Right=�$��0,10,0,0q�.+��� -   #��[  .���� $ 
Horizontal=�.+��� $��Segoe MDL2 Assets)�$	����$��0,0,5,0q��� $��Descargar Log���� -   #��[  .���� $ 
Horizontal=�.+��� $��Segoe MDL2 Assets)�$	����$��0,0,5,0q��� $��Salir���� -   #��[  .���� $ 
Horizontal=�.+��� $��Segoe MDL2 Assets)�$	����$��0,0,5,0q��� $��Limpiar Log��� -    �Name$ ToastContainer��$��Bottom=�$��Center=�$��0,0,0,20q�$y�	Collapsed=� K� ZIndex$
 1000��.+��� ���#28a745$ 6�� �� Padding$
 10,6q� J� Effect# �  ._��� $ 
Horizontal=�.+��� $	��✔��$��16(�$
��Bold"�
�������$��0,0,6,0q��� -    ��Name$ ToastMessage��$��14(�
�������