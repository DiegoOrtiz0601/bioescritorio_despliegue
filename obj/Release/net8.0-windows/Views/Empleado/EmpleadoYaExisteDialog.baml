   M S B A M L   `   `   `  ����   BiomentricoHolding>    8BiomentricoHolding.Views.Empleado.EmpleadoYaExisteDialog   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Empleado ya existe��$	��220��$	��420�� � WindowStartupLocation$ CenterScreen=�
������  � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � AllowsTransparency$
 True�� � Topmost$
 True��.��� ���	#80000000.+��� 
������� �� CornerRadius$ 16�� �� Padding$ 25q�$	��380��$��Center=�$��Center=�   
DropShadow J� Effect# �   ._��� .+��� $#��⚠️ Empleado ya registrado��$��18(�$
��Bold"����#c70100	 �� TextAlignment$	 Center=�$��0,0,0,10q��� -   
 ��Name$
 
txtMensaje��$2��,El empleado ya existe.\n¿Qué deseas hacer?��$��14(�$	 Center=�$
��Wrap=�$��0,0,0,20q����#1F2937��  �� Orientation$ 
Horizontal=�$��Center=�$��0,10,0,0q�.+��� -    ��Name$ btnModificar��$��✏️ Modificar��$	��140��$��35��$
��Bold"�
����������#680307 � Cursor$
 Hand��$��0,0,10,0q�����  �� 
TargetType" �� �� BasedOn# [B��.���  �� Property � �� �� Value ��  �� 
TargetType" ��.R��� #��z��$ 8��$ 10q�._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � �� �#343a40�� -   $ 	btnCerrar��$��
❌ Cerrar��$	��100��$��35��$
��Bold"����#E5E7EB���#1F2937$
 Hand������ " ��# [B��.���  � �� �� " ��.R��� #��z��$ 8��$ 10q�._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �#D1D5DB