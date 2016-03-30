package handy;

import java.awt.datatransfer.DataFlavor;
import java.awt.dnd.DnDConstants;
import java.awt.dnd.DropTarget;
import java.awt.dnd.DropTargetDropEvent;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Collection;
import java.util.Comparator;
import java.util.List;
import java.util.Vector;
import java.io.File;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;



/**
 * @author HandyMain
 *
 */
public class MainFrame extends JFrame {
	///////////////////////////////////////////////////
	// 필드를 정의합니다.
	private static final long serialVersionUID = 1L;
	
	// 
	JTable table;
	
	// 필드 정의의 끝
	///////////////////////////////////////////////////
	
	
	
	///////////////////////////////////////////////////
	// 생성자를 정의합니다.
	public MainFrame() {
		initAction();
		initMenuBar();
		initTable();
		
		
		
		originFiles = new Vector<File>();
		this.setLocation(100, 100);
	}
	
	// 생성자 정의의 끝
	///////////////////////////////////////////////////
	
	
	
	///////////////////////////////////////////////////
	// 초기화 메서드를 정의합니다.
	ActionListener actionFileClearList;
	ActionListener actionFileSort;
	ActionListener actionFileApply;
	ActionListener actionFileRemoveSelectedExt;
	ActionListener actionFileRemoveExceptSelectedExt;
	ActionListener actionFileClose;
	ActionListener actionSettingStyle;
	ActionListener actionSettingDeveloper;
	
	/**
	 * 이벤트 핸들러를 초기화 합니다.
	 */
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
		actionSettingStyle = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
		actionSettingDeveloper = new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
			}
		};
	}	
	/**
	 * 메뉴 바를 초기화 합니다.
	 */
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
		menuItem.addActionListener(actionSettingStyle);
		menu.add(menuItem);
		
		menuItem = new JMenuItem("Developer");
		menuItem.addActionListener(actionSettingDeveloper);
		menu.add(menuItem);
		mbar.add(menu);
		
		setJMenuBar(mbar);
	}
	/**
	 * initialize table object
	 */
	void initTable() {
		DropTarget tableDropTarget = new DropTarget() {
			private static final long serialVersionUID = 1L;
			
			/** (non-Javadoc)
			 * @see java.awt.dnd.DropTarget#drop(java.awt.dnd.DropTargetDropEvent)
			 */
			public synchronized void drop(DropTargetDropEvent e) {
				try {
					e.acceptDrop(DnDConstants.ACTION_COPY);
					// List<File> transferData = ;
					addFiles((List<File>)
							e.getTransferable().getTransferData
							(DataFlavor.javaFileListFlavor));
					
					/*
					List<File> droppedFiles = transferData;
					
					for (File file: droppedFiles) {
						String path = file.getAbsolutePath();
						System.out.printf("%s \n", path);
						DefaultTableModel model = (DefaultTableModel) table.getModel();
						model.addRow(new Object[]{path, "new one"});
					}
					*/
				}
				catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		};
		
		DefaultTableModel dtm = new DefaultTableModel(1, 2);
		table = new JTable(dtm);
		table.setDropMode(DropMode.INSERT_ROWS);
		table.setAutoCreateRowSorter(true);
		table.setRowSelectionAllowed(true);
		table.getTableHeader().setDropTarget(tableDropTarget);
		table.setDropTarget(tableDropTarget);

		JScrollPane scrollPane = new JScrollPane(table);
		getContentPane().add(scrollPane);
		setSize(1000, 800);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	
	// 이벤트 핸들러 정의의 끝
	///////////////////////////////////////////////////
	
	
	
	private Comparator<File> generalComparator;
	private Comparator<File> leftIndexComparator;
	private Comparator<File> rightIndexComparator;
	
	
	
	///////////////////////////////////////////////////
	// 내부 메서드를 정의합니다.
	Vector<File> originFiles;
	
	void addFile(File file) {
		originFiles.add(file);
		updateTableContents();
	}
	void addFiles(Collection<File> files) {
		originFiles.addAll(files);
		
		DefaultTableModel model = (DefaultTableModel) table.getModel();
		for (File file: files) {
			String path = file.getAbsolutePath();
			System.out.printf("%s \n", path);
			model.addRow(new Object[]{path, "new one"});
		}
		
		updateTableContents();
	}
	
	void updateTableContents() {
		setGeneralComparator((File arg0, File arg1) -> {
			String name1 = arg0.getAbsolutePath();
			String name2 = arg1.getAbsolutePath();
			return name1.compareTo(name2);
		});
		originFiles.sort((Comparator<File>) getGeneralComparator());
	}
	public Comparator<File> getGeneralComparator() {
		return generalComparator;
	}
	public void setGeneralComparator(Comparator<File> generalComparator) {
		this.generalComparator = generalComparator;
	}
	
	// 
	///////////////////////////////////////////////////
}
