package handy;

import java.awt.Component;
import java.awt.datatransfer.DataFlavor;
import java.awt.dnd.DnDConstants;
import java.awt.dnd.DropTarget;
import java.awt.dnd.DropTargetDropEvent;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import java.util.List;
import java.io.File;


import javax.swing.*;
import javax.swing.event.TableModelListener;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.JTableHeader;
import javax.swing.table.TableModel;

public class MainFrame extends JFrame {
	private static final long serialVersionUID = 1L;
	
	JTable table;
	
	
	public MainFrame() {
		initAction();
		initMenuBar();
		initTable();
		
		this.setLocation(100, 100);
	}
	
	ActionListener actionFileClearList;
	ActionListener actionFileSort;
	ActionListener actionFileApply;
	ActionListener actionFileRemoveSelectedExt;
	ActionListener actionFileRemoveExceptSelectedExt;
	ActionListener actionFileClose;
	ActionListener actionSettingStyle;
	ActionListener actionSettingDeveloper;
	
	void initAction() {
		actionFileClearList = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		actionFileSort = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		actionFileApply = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		actionFileRemoveSelectedExt = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		actionFileRemoveExceptSelectedExt = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		
	}
	
	void initMenuBar() {
		JMenuBar mbar = new JMenuBar();
		JMenu menu;
		JMenuItem menuItem;
		ActionListener action = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		
		// menu: File
		menu = new JMenu("파일");
		menuItem = new JMenuItem("목록 비우기");
		menuItem.addActionListener(actionFileClearList);
		menuItem.setAccelerator(KeyStroke.getKeyStroke('c'));
		menu.add(menuItem);
		
		menuItem = new JMenuItem("정렬");
		menuItem.addActionListener(actionFileSort);
		menu.add(menuItem);

		menuItem = new JMenuItem("적용");
		menuItem.addActionListener(actionFileApply);
		menu.add(menuItem);
		
		menuItem = new JMenuItem("지정한 확장자를 갖는 파일 제외");
		menuItem.addActionListener(actionFileRemoveSelectedExt);
		menu.add(menuItem);
		
		menuItem = new JMenuItem("지정한 확장자 외의 것을 갖는 파일 제외");
		menuItem.addActionListener(actionFileRemoveExceptSelectedExt);
		menu.add(menuItem);
		
		menu.add(new JSeparator());
		
		menuItem = new JMenuItem("닫기");
		menuItem.addActionListener(action);
		menu.add(menuItem);
		mbar.add(menu);
		
		// menu: Setting
		menu = new JMenu("Setting");
		menuItem = new JMenuItem("Style");
		menuItem.addActionListener(action);
		menu.add(menuItem);
		
		menuItem = new JMenuItem("Developer");
		menuItem.addActionListener(action);
		menu.add(menuItem);
		mbar.add(menu);
		
		setJMenuBar(mbar);
	}
	
	
	
	/**
	 * initialize table object
	 */
	void initTable() {
		/*
	    String[] columnNames = { 
		    	"변경 이전",
		    	"변경 이후",
		};
		String[][] data = { {"",""} };
		*/
		
		
		DropTarget tableDropTarget = new DropTarget() {
			/**
			 * 
			 */
			private static final long serialVersionUID = 1L;

			public synchronized void drop(DropTargetDropEvent e) {
				try {
					e.acceptDrop(DnDConstants.ACTION_COPY);
					List<File> transferData = (List<File>)
							e.getTransferable().getTransferData
							(DataFlavor.javaFileListFlavor);
					List<File> droppedFiles = transferData;
					
					for (File file: droppedFiles) {
						String path = file.getAbsolutePath();
						System.out.printf("%s \n", path);
						DefaultTableModel model = (DefaultTableModel) table.getModel();
						model.addRow(new Object[]{path, "new one"});
					}
				}
				catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		};
		
		
		
		DefaultTableModel dtm = new DefaultTableModel(0, 2);
		table = new JTable(dtm);
		table.setDropMode(DropMode.INSERT_ROWS);
		table.setAutoCreateRowSorter(true);
		table.setRowSelectionAllowed(true);
		table.getTableHeader().setDropTarget(tableDropTarget);
		table.setDropTarget(tableDropTarget);

		JScrollPane scrollPane = new JScrollPane(table);
		getContentPane().add(scrollPane);
		setSize(600, 400);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	
	
	
	void _initTable() {
	    String[] columnNames = { 
		    	"이름",
		    	"이메일",
		    	"휴대폰", 
		    	"성별"
		};
		String[][] data = {
		    { "홍길동", "hong@ah.co.kr", "010-1111-1112", "남" },
		};
		table = new JTable(data, columnNames);
		table.setDropMode(DropMode.INSERT_ROWS);
		table.setAutoCreateRowSorter(true);
		table.setRowSelectionAllowed(true);
		
		table.setDropTarget(new DropTarget() {
			/**
			 * 
			 */
			private static final long serialVersionUID = 1L;

			public synchronized void drop(DropTargetDropEvent e) {
				try {
					e.acceptDrop(DnDConstants.ACTION_COPY);
					List<File> transferData = (List<File>)
							e.getTransferable().getTransferData
							(DataFlavor.javaFileListFlavor);
					List<File> droppedFiles = transferData;
					
					for (File file: droppedFiles) {
						System.out.printf("%s", file.getAbsolutePath());
						
					}
				}
				catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		});

		JScrollPane scrollPane = new JScrollPane(table);
		getContentPane().add(scrollPane);
		setSize(600, 400);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
}
