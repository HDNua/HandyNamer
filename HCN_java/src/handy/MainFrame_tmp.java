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
	// declare fields
	private static final long serialVersionUID = 1L;
	
	// table holds original path name's and 
	JTable table;
	
	// end of field declaration
	///////////////////////////////////////////////////
	
	
	
	///////////////////////////////////////////////////
	// define constructor
	public MainFrame() {
		initAction();
		initMenuBar();
		initTable();
		
		
		
		originFiles = new Vector<File>();
		this.setLocation(100, 100);
	}
	
	// end of constructor definition
	///////////////////////////////////////////////////
	
	
	
	///////////////////////////////////////////////////
	// define event handlers
	ActionListener actionFileClearList;
	ActionListener actionFileSort;
	ActionListener actionFileApply;
	ActionListener actionFileRemoveSelectedExt;
	ActionListener actionFileRemoveExceptSelectedExt;
	ActionListener actionFileClose;
	ActionListener actionSettingStyle;
	ActionListener actionSettingDeveloper;
	
	/**
	 * initialize event handler actions
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
	 * initialize menu bar and items of it
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
		menu = new JMenu("¿¿");
		menuItem = new JMenuItem("¿¿ ¿¿¿");
		menuItem.addActionListener(actionFileClearList);
		menuItem.setAccelerator(KeyStroke.getKeyStroke('c'));
		menu.add(menuItem);
		
		menuItem = new JMenuItem("¿¿");
		menuItem.addActionListener(actionFileSort);
		menu.add(menuItem);

		menuItem = new JMenuItem("¿¿");
		menuItem.addActionListener(actionFileApply);
		menu.add(menuItem);
		
		menuItem = new JMenuItem("¿¿¿ ¿¿¿¿ ¿¿ ¿¿ ¿¿");
		menuItem.addActionListener(actionFileRemoveSelectedExt);
		menu.add(menuItem);
		
		menuItem = new JMenuItem("¿¿¿ ¿¿¿ ¿¿ ¿¿ ¿¿ ¿¿ ¿¿");
		menuItem.addActionListener(actionFileRemoveExceptSelectedExt);
		menu.add(menuItem);
		
		menu.add(new JSeparator());
		
		menuItem = new JMenuItem("¿¿");
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
	
	// end of event handler definition
	///////////////////////////////////////////////////
	
	
	
	private Comparator<File> generalComparator;
	private Comparator<File> leftIndexComparator;
	private Comparator<File> rightIndexComparator;
	
	
	
	///////////////////////////////////////////////////
	// inner methods
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
