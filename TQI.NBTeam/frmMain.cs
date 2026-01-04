using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TQI.NBTeam.Commons;
using TQI.NBTeam.Containers;
using TQI.NBTeam.Handlers;
using TQI.NBTeam.Helpers;
using TQI.NBTeam.Models;
using TQI.NBTeam.Services;

namespace TQI.NBTeam;

public class frmMain : Form
{
    private CancellationTokenSource _cts = new CancellationTokenSource();

    private List<Task> _loginTasks;

    private IContainer components = null;

    private Panel panel1;

    private TabControl tabControl1;

    private TabPage tabPage1;

    private TabPage tabPage2;

    private ToolStrip tsVia;

    private TabPage tabPage3;

    private DataGridView dtgvVia;

    private DataGridViewTextBoxColumn cStt;

    private DataGridViewTextBoxColumn cUid;

    private DataGridViewTextBoxColumn cPassword;

    private DataGridViewTextBoxColumn cKey2FA;

    private DataGridViewTextBoxColumn cCookie;

    private DataGridViewTextBoxColumn cToken;

    private DataGridViewTextBoxColumn cProcess;

    private DataGridView dtgvBM;

    private ToolStrip tsBM;

    private DataGridView dtgvTKQC;

    private ToolStrip tsTKQC;

    private ContextMenuStrip ctmnsVia;

    private ToolStripMenuItem dánViaToolStripMenuItem;

    private TabControl tabControl2;

    private TabPage tabPage4;

    private TabPage tabPage5;

    private Button btnStop;

    private Button btnStart;

    private Label label1;

    private ComboBox cbbTypeLogin;

    private Label label4;

    private TextBox textBox1;

    private Label label3;

    private ComboBox cbbTypeProxy;

    private NumericUpDown nudCountThreads;

    private Label label2;

    private Label label5;

    private RichTextBox rtbLog;

    private ToolStripLabel lblStatus;

    private ToolStripLabel lblTextTotalCount;

    private ToolStripLabel lblTotalCount;

    private ToolStripLabel toolStripLabel1;

    private ToolStripLabel lblCountSelected;

    private ToolStripMenuItem xóaViaToolStripMenuItem;

    private GroupBox groupBox1;

    private Button btnLoadTKQC;

    private ComboBox cbbTypeLoadTKQC;

    private Button btnLoadBM;

    private ComboBox cbbTypeLoadBM;

    private ToolStripLabel toolStripLabel2;

    private ToolStripLabel lblCountTotalBM;

    private ContextMenuStrip ctmnsBM;

    private ToolStripMenuItem dánIDToolStripMenuItem;

    private ToolStripMenuItem loadTKQCToolStripMenuItem;

    private ToolStripMenuItem checkInfoToolStripMenuItem;

    private ToolStripMenuItem loadUserToolStripMenuItem;

    private ToolStripMenuItem loadAssetToolStripMenuItem;

    private GroupBox groupBox2;

    private GroupBox groupBox3;

    private ComboBox cbbTypeRole;

    private Label label7;

    private ComboBox cbbTypeMail;

    private Label label6;

    private Button btnBackUp;

    private GroupBox groupBox4;

    private Button btnCreateAdAccount;

    private Label label9;

    private TextBox tbPartner;

    private ComboBox cbbTypeCreateAdAccount;

    private Label label8;

    private TabPage tabPage6;

    private Label lblCountLinkBM;

    private Label label10;

    private RichTextBox rtbLinkBM;

    private TabControl tabControl3;

    private TabPage tabPage7;

    private TabPage tabPage8;

    private TableLayoutPanel tableLayoutPanel1;

    private TextBox tbRegion;

    private Label label13;

    private ComboBox cbbCurrencyBM;

    private ComboBox cbbTimeZoneBM;

    private Label label12;

    private Label label11;

    private TextBox tbNameBM;

    private Label label14;

    private ContextMenuStrip ctmnsTKQC;

    private ToolStripMenuItem toolStripMenuItem1;

    private ToolStripMenuItem toolStripMenuItem2;

    private ToolStripMenuItem thoátTKQCToolStripMenuItem;

    private ToolStripMenuItem xóaQTVToolStripMenuItem;

    private ToolStripMenuItem đóngTKQCToolStripMenuItem;

    private ToolStripMenuItem mởTKQCToolStripMenuItem;

    private ToolStripMenuItem tsmPasteBusinessId;

    private GroupBox groupBox5;

    private TabControl tabControl4;

    private TabPage tabPage9;

    private Label label16;

    private ComboBox cbbPermitTask;

    private Label label15;

    private TextBox tbPartnerId;

    private GroupBox groupBox6;

    private Button btnAssignPartner;

    private Button btnRemovePartner;

    private DataGridView dtgvUser;

    private ContextMenuStrip ctmnsUser;

    private ToolStripMenuItem xóaQuảnTrịToolStripMenuItem;

    private ToolStripMenuItem xóaQuảnTrịTrước7NgàyToolStripMenuItem;

    private ToolStripMenuItem xóaLờiMờiToolStripMenuItem;

    private ToolStripMenuItem hạQuyềnToolStripMenuItem;

    private DataGridViewTextBoxColumn cSttUser;

    private DataGridViewTextBoxColumn cUidUser;

    private DataGridViewTextBoxColumn cUserId;

    private DataGridViewTextBoxColumn cBMIdUser;

    private DataGridViewTextBoxColumn cMailUser;

    private DataGridViewTextBoxColumn cNameUser;

    private DataGridViewTextBoxColumn cRoleUser;

    private DataGridViewTextBoxColumn cStatusUser;

    private TabPage tabPage11;

    private Label label17;

    private TextBox tbBusinessName;

    private Button btnChangeNameBM;

    private ToolStripMenuItem thoátTKQCToolStripMenuItem1;

    private ToolStripMenuItem loadLiveToolStripMenuItem;

    private ToolStripMenuItem loadDieToolStripMenuItem;

    private ToolStripLabel toolStripLabel3;

    private ToolStripLabel lblTotalCountTKQC;

    private ToolStripMenuItem kíchAppToolStripMenuItem;

    private ToolStripMenuItem gánQuyềnAddThẻToolStripMenuItem;

    private Label label18;

    private TextBox tbBMID;

    private Button btnBuildBM;

    private ToolStripMenuItem mởBMToolStripMenuItem;

    private Label label19;

    private NumericUpDown nudCountNumAdAccount;

    private Button btnOutBM;

    private ToolStripMenuItem xóaLờiMờiToolStripMenuItem1;

    private ToolStripMenuItem checkBMToolStripMenuItem;

    private Button btnChangeEmail;

    private Label label20;

    private TextBox tbMailBM;

    private TabPage tabPage13;

    private GroupBox groupBox7;

    private GroupBox groupBox8;

    private Button btnSetGHCT;

    private TextBox tbGHCT;

    private ToolStripMenuItem checkCampaignToolStripMenuItem;

    private ToolStripMenuItem checkBillToolStripMenuItem;

    private ToolStripMenuItem payToolStripMenuItem;

    private TabPage tabPage10;

    private Button btnJoinBM;

    private TextBox tbLinkBM;

    private GroupBox groupBox9;

    private Button btnShareTKQC;

    private TextBox tbLstUidVia;

    private ToolStripMenuItem checkInfoBMToolStripMenuItem;

    private ToolStripMenuItem checkLimitBMToolStripMenuItem;

    private Button btnRemoveGHCT;

    private DataGridViewTextBoxColumn cSttBM;

    private DataGridViewTextBoxColumn cIDBMVia;

    private DataGridViewTextBoxColumn cUidVia;

    private DataGridViewTextBoxColumn cNameBM;

    private DataGridViewTextBoxColumn cBMType;

    private DataGridViewTextBoxColumn cVerifyBM;

    private DataGridViewTextBoxColumn cCreateAdAccountBM;

    private DataGridViewTextBoxColumn cCountQTVBM;

    private DataGridViewTextBoxColumn cInfoBM;

    private DataGridViewTextBoxColumn cCreateTimeBM;

    private DataGridViewTextBoxColumn cStatusBM;

    private DataGridViewTextBoxColumn cLimitAdAccountBM;

    private DataGridViewTextBoxColumn cAdAccountCount;

    private DataGridViewTextBoxColumn cQTVBM;

    private DataGridViewTextBoxColumn cProcessBM;

    private ToolStripMenuItem mởChromePEToolStripMenuItem;

    private ToolStripMenuItem tạoTKQCToolStripMenuItem;

    private DataGridViewTextBoxColumn cSttTKQC;

    private DataGridViewTextBoxColumn cIDTKQC;

    private DataGridViewTextBoxColumn cUidViaTKQC;

    private DataGridViewTextBoxColumn cNameTKQC;

    private DataGridViewTextBoxColumn cAccountSpent;

    private DataGridViewTextBoxColumn cStatusTKQC;

    private DataGridViewTextBoxColumn cCurrencyTKQC;

    private DataGridViewTextBoxColumn cLimitTKQC;

    private DataGridViewTextBoxColumn cThreshold;

    private DataGridViewTextBoxColumn cBalanceTKQC;

    private DataGridViewTextBoxColumn cSpendCap;

    private DataGridViewTextBoxColumn cPaymentTKQC;

    private DataGridViewTextBoxColumn cTimeZoneTKQC;

    private DataGridViewTextBoxColumn cBusiness;

    private DataGridViewTextBoxColumn cCreateTimeTKQC;

    private DataGridViewTextBoxColumn cOwnerTKQC;

    private DataGridViewTextBoxColumn cRegionTKQC;

    private DataGridViewTextBoxColumn cUserCountTKQC;

    private DataGridViewTextBoxColumn cPartnerCount;

    private DataGridViewTextBoxColumn cCampaignTKQC;

    private DataGridViewTextBoxColumn cBussinessId;

    private DataGridViewTextBoxColumn cProcessTKQC;

    private ToolStripMenuItem checkQTVBMToolStripMenuItem;

    private TextBox tbMailPass;

    private Label label21;

    private ToolStripMenuItem kíchNútKhángToolStripMenuItem;

    private Button btnGetCookieIG;

    private ToolStripMenuItem tạoWAToolStripMenuItem;

    private TabPage tabPage12;

    private Label label22;

    private TextBox tbCaptchaKey;

    private Button btnAppeal;

    private Label label23;

    private TextBox tbViotpApiKey;

    private ToolStripMenuItem xóaWAToolStripMenuItem;

    private ToolStripMenuItem checkWAToolStripMenuItem;

    private ToolStripMenuItem cHECKTKQCDIEToolStripMenuItem;

    private ToolStripMenuItem loadTKẨnToolStripMenuItem;

    private TabPage tabPage14;

    private Button btnCreateWA;

    private Label label24;

    private NumericUpDown nudCountCreateWA;

    private ToolStripMenuItem checkLimitTKẨnToolStripMenuItem;

    private ToolStripMenuItem nhétTKQCToolStripMenuItem;

    private ToolStripMenuItem mởBMChromeToolStripMenuItem;

    private ToolStripMenuItem xóaQuảnTrịBằngIGToolStripMenuItem;

    private ToolStripMenuItem kíchAppToolStripMenuItem1;

    private TabPage tabPage15;

    private GroupBox groupBox10;

    private ComboBox cbbDomain;

    private Label label26;

    private Label label25;

    private TextBox tbUsername;

    private DataGridView dtgvHotmail;

    private Button btnCheckMail;

    private TabControl tabControl5;

    private TabPage tabPage16;

    private TabPage tabPage17;

    private WebBrowser webBrowser1;

    private Button btnDeleteAll;

    private ContextMenuStrip ctmnsHotmail;

    private ToolStripMenuItem copyLinkToolStripMenuItem;

    private DataGridViewTextBoxColumn cSttMessage;

    private DataGridViewTextBoxColumn cSender;

    private DataGridViewTextBoxColumn cReceiveTime;

    private DataGridViewTextBoxColumn cSubject;

    private DataGridViewTextBoxColumn cCode;

    private DataGridViewTextBoxColumn cBody;

    private DataGridViewTextBoxColumn cBusinessLink;

    private CheckBox ckbGetLinkBM;

    private ToolStripMenuItem xóaQTVIGToolStripMenuItem;

    private Label label27;

    private TextBox tbProxy;

    private ToolStripMenuItem kíchAppIGToolStripMenuItem;

    private TabPage tabPage18;

    private DataGridView dtgvIG;

    private ContextMenuStrip ctmsIG;

    private ToolStripMenuItem dánIGToolStripMenuItem;

    private ToolStripMenuItem checkIGToolStripMenuItem;

    private ToolStripMenuItem checkKíchToolStripMenuItem;

    private DataGridViewTextBoxColumn cSttIG;

    private DataGridViewTextBoxColumn cUsernameIG;

    private DataGridViewTextBoxColumn cPasswordIG;

    private DataGridViewTextBoxColumn cKey2FAIG;

    private DataGridViewTextBoxColumn cCookieIG;

    private DataGridViewTextBoxColumn cProcessIG;

    private ToolStripMenuItem thêmVàoDòng2BMToolStripMenuItem;

    private TabPage tabPage19;

    private TabPage tabPage20;

    private TextBox tbNameAdAccount;

    private Label label28;

    private TextBox tbRegionAdAccount;

    private Label label29;

    private ComboBox cbbCurrencyAdAccount;

    private ComboBox cbbTimeZoneAdAccount;

    private Label label30;

    private Label label31;

    private Button btnChangeInforAdAccount;

    private Button btnAddPermission;

    private Button btnLoadBusinessUserName;

    private ComboBox cbbBusinessUserName;

    private Label label32;

    private TextBox tbBusinessId;

    private ToolStripMenuItem checkPTTTToolStripMenuItem;

    private ToolStripMenuItem createRuleToolStripMenuItem;

    private ToolStripMenuItem deleteCreditCardToolStripMenuItem;

    private ToolStripMenuItem deleteCampToolStripMenuItem;

    public frmMain()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Onload();
    }

    private void Onload()
    {
        cbbTypeLogin.SelectedIndex = 0;
        cbbTypeProxy.SelectedIndex = 0;
        cbbTypeLoadBM.SelectedIndex = 0;
        cbbTypeLoadTKQC.SelectedIndex = 0;
        Text = $"TQI.NBTeamAds v{Application.ProductVersion}";
        dtgvVia.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        dtgvVia.RowTemplate.Height = 35;
        dtgvBM.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        dtgvBM.RowTemplate.Height = 35;
        dtgvTKQC.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        dtgvTKQC.RowTemplate.Height = 35;
        LoadTimeZone();
        LoadCurrency();
        LoadTypeMail();
        LoadTypeCreateAdAccount();
        LoadPermitTask();
        LoadTypeMailRead();
        tbNameAdAccount.Text = "TQI-" + DateTime.Now.ToString("dd/MM/yyyy");
    }

    private void LoadTimeZone()
    {
        string filePath = Path.Combine(Application.StartupPath, "lib", "Data", "Timezone.json");
        Dictionary<string, string> timeZoneDict = JArray.Parse(File.ReadAllText(filePath)).ToDictionary((JToken timeZone) => timeZone["Id"].ToString(), (JToken timeZone) => timeZone["Name"].ToString());
        cbbTimeZoneBM.DataSource = new BindingSource(timeZoneDict, null);
        cbbTimeZoneBM.ValueMember = "Key";
        cbbTimeZoneBM.DisplayMember = "Value";
        cbbTimeZoneAdAccount.DataSource = new BindingSource(timeZoneDict, null);
        cbbTimeZoneAdAccount.ValueMember = "Key";
        cbbTimeZoneAdAccount.DisplayMember = "Value";
    }

    private void LoadCurrency()
    {
        string filePath = Path.Combine(Application.StartupPath, "lib", "Data", "Currency.json");
        Dictionary<string, string> currencyDict = JArray.Parse(File.ReadAllText(filePath)).ToDictionary((JToken currency) => currency["Id"].ToString(), (JToken currentcy) => currentcy["Name"].ToString());
        cbbCurrencyBM.DataSource = new BindingSource(currencyDict, null);
        cbbCurrencyBM.ValueMember = "Key";
        cbbCurrencyBM.DisplayMember = "Value";
        cbbCurrencyBM.SelectedValue = "51";
        cbbCurrencyAdAccount.DataSource = new BindingSource(currencyDict, null);
        cbbCurrencyAdAccount.ValueMember = "Key";
        cbbCurrencyAdAccount.DisplayMember = "Value";
        cbbCurrencyAdAccount.SelectedValue = "51";
    }

    private void LoadTypeMail()
    {
        Dictionary<string, string> typeMailDict = new Dictionary<string, string>
        {
            { "1", "Moakt" },
            { "2", "Mailngon.top" },
            { "3", "Fviainboxes" },
            { "4", "Nhập Mail" }
        };
        cbbTypeMail.DataSource = new BindingSource(typeMailDict, null);
        cbbTypeMail.ValueMember = "Key";
        cbbTypeMail.DisplayMember = "Value";
    }

    private void LoadTypeMailRead()
    {
        Dictionary<string, string> typeMailDict = new Dictionary<string, string>
        {
            { "1", "Hotmail" },
            { "2", "Mailngon.top" },
            { "3", "Moakt" }
        };
        cbbDomain.DataSource = new BindingSource(typeMailDict, null);
        cbbDomain.ValueMember = "Key";
        cbbDomain.DisplayMember = "Value";
    }

    private void LoadTypeCreateAdAccount()
    {
        Dictionary<string, string> typeCreateAdAccountDict = new Dictionary<string, string>
        {
            { "1", "Tạo TKQC" },
            { "2", "Tạo TKQC Share đối tác" }
        };
        cbbTypeCreateAdAccount.DataSource = new BindingSource(typeCreateAdAccountDict, null);
        cbbTypeCreateAdAccount.ValueMember = "Key";
        cbbTypeCreateAdAccount.DisplayMember = "Value";
    }

    private void LoadPermitTask()
    {
        Dictionary<string, string> permitTask = new Dictionary<string, string>
        {
            { "1", "Đối tác" },
            { "2", "Nhân viên" }
        };
        cbbPermitTask.DataSource = new BindingSource(permitTask, null);
        cbbPermitTask.ValueMember = "Key";
        cbbPermitTask.DisplayMember = "Value";
    }

    private void dánViaToolStripMenuItem_Click(object sender, EventArgs e)
    {
        dtgvVia.Rows.Clear();
        string clipboard = Clipboard.GetText();
        if (string.IsNullOrEmpty(clipboard))
        {
            ShowError("Không có dữ liệu trong clipboard");
            return;
        }
        Task.Run(delegate
        {
            ProcessClipboardData(clipboard, dtgvVia);
        });
    }

    private void ProcessClipboardData(string clipboardText, DataGridView dgv)
    {
        try
        {
            string[] lines = (from x in clipboardText.Split('\n')
                              where !string.IsNullOrEmpty(x)
                              select x).ToArray();
            Parallel.ForEach(lines, delegate (string line)
            {
                ProcessDataLine(dgv, line);
            });
        }
        catch (Exception)
        {
            ShowError("Lỗi dán thông tin via. Vui lòng thử lại!");
        }
    }

    private void ProcessDataLine(DataGridView dgv, string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            string[] dataRaw = data.Split('|');
            if (dataRaw.Length >= 3)
            {
                AddFullDataRow(dgv, dataRaw);
            }
            else if (dataRaw.Length == 1 && data.Contains("c_user"))
            {
                AddCookieOnlyRow(dgv, data);
            }
        }
    }

    private void AddFullDataRow(DataGridView dgv, string[] dataRaw)
    {
        SafeInvoke(delegate
        {
            int index = dgv.Rows.Add();
            DataGridViewRow dataGridViewRow = dgv.Rows[index];
            string text = dataRaw.FirstOrDefault((string x) => x.Contains("c_user"));
            string token = dataRaw.FirstOrDefault((string x) => x.Contains("EAA"));
            dataGridViewRow.Cells[0].Value = dgv.RowCount;
            dataGridViewRow.Cells[1].Value = dataRaw[0];
            dataGridViewRow.Cells[2].Value = dataRaw[1];
            dataGridViewRow.Cells[3].Value = ((dataRaw[2].Length == 32) ? dataRaw[2] : "");
            dataGridViewRow.Cells[4].Value = text;
            dataGridViewRow.Cells[5].Value = dataRaw.FirstOrDefault((string x) => x.Contains("EAA"));
            AccountDto account = new AccountDto
            {
                Uid = dataRaw[0],
                Password = dataRaw[1],
                Key2FA = ((dataRaw[2].Length == 32) ? dataRaw[2] : ""),
                Cookie = text,
                Token = token
            };
            FacebookHandlerContainer.Instance.FacebookHandlerProfile.Add(new FacebookHandlerDto
            {
                Account = account
            });
        }, dtgvVia);
    }

    private void AddCookieOnlyRow(DataGridView dgv, string cookie)
    {
        string uid = Regex.Match(cookie, "c_user=(.*?);").Groups[1].Value;
        SafeInvoke(delegate
        {
            int index = dgv.Rows.Add();
            DataGridViewRow dataGridViewRow = dgv.Rows[index];
            dataGridViewRow.Cells[0].Value = dgv.RowCount;
            dataGridViewRow.Cells[1].Value = uid;
            dataGridViewRow.Cells[4].Value = cookie;
            AccountDto account = new AccountDto
            {
                Uid = uid,
                Cookie = cookie
            };
            FacebookHandlerContainer.Instance.FacebookHandlerProfile.Add(new FacebookHandlerDto
            {
                Account = account
            });
        }, dtgvVia);
    }

    private void SafeInvoke(Action action, DataGridView dgv)
    {
        if (dgv.InvokeRequired)
        {
            dgv.Invoke(action);
        }
        else
        {
            action();
        }
    }

    private void ShowError(string message)
    {
        if (base.InvokeRequired)
        {
            Invoke((Action)delegate
            {
                MessageBox.Show(message, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            });
        }
        else
        {
            MessageBox.Show(message, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        _cts = new CancellationTokenSource();
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvVia.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản cần Login!!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeLogin = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string proxy = string.Empty;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            AccountDto accountDto = GetAccountDtoByRow(row);
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    await ProcessLoginFacebook(accountDto, row, proxy, typeLogin);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Login: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore?.Release();
                }
            }));
        }
    }

    private async void btnStop_Click(object sender, EventArgs e)
    {
        _cts.Cancel();
        await UpdateStatusAsync("Status: Stopping...");
        if (_loginTasks != null && _loginTasks.Any())
        {
            await Task.WhenAll(_loginTasks).WithTimeout(TimeSpan.FromSeconds(30.0));
        }
        CleanupResources();
        await UpdateStatusAsync("Status: Stopped");
    }

    private async Task ProcessLoginFacebook(AccountDto accountDto, DataGridViewRow row, string proxy, int typeLogin)
    {
        UpdateGridCellAsync(row, "cProcess", "Logging...", 0);
        FacebookHandler facebookHandler = new FacebookHandler(accountDto, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", proxy);
        facebookHandler.LoginCookie();
        UpdateGridCellAsync(row, "cProcess", "Log in Success => Get token", 0);
        var (dtsg, token) = await facebookHandler.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            row.Cells["cToken"].Value = token;
            string cookie = facebookHandler.GetCookie();
            accountDto.Cookie = cookie;
            accountDto.DTSGToken = dtsg;
            accountDto.Token = token;
            UpdateGridCellAsync(row, "cProcess", "Get Token Done", 0);
            FacebookHandlerDto facebookHandlerDto = new FacebookHandlerDto
            {
                FacebookHandler = facebookHandler,
                Account = accountDto,
                IsLoggedIn = true
            };
            FacebookHandlerContainer.Instance.AddFacebookHandlerProfile(facebookHandlerDto);
        }
        else
        {
            UpdateGridCellAsync(row, "cProcess", "Get Token Failed", 1);
        }
    }

    private AccountDto GetAccountDtoByRow(DataGridViewRow row)
    {
        return new AccountDto
        {
            Uid = row.Cells[1].Value?.ToString(),
            Password = row.Cells[2].Value?.ToString(),
            Key2FA = row.Cells[3].Value?.ToString(),
            Cookie = row.Cells[4].Value?.ToString()
        };
    }

    private async Task UpdateStatusAsync(string message)
    {
        if (base.InvokeRequired)
        {
            await Task.Run(() => Invoke((Action)delegate
            {
                UpdateStatusAsync(message).GetAwaiter().GetResult();
            }));
            return;
        }
        lblStatus.Text = message;
        if (message.Contains("Running"))
        {
            lblStatus.ForeColor = Color.Green;
        }
        else
        {
            lblStatus.ForeColor = Color.Red;
        }
    }

    private async Task UpdateLabelText(ToolStripLabel label, string message)
    {
        if (base.InvokeRequired)
        {
            await Task.Run(() => Invoke((Action)delegate
            {
                UpdateLabelText(label, message).GetAwaiter().GetResult();
            }));
        }
        else
        {
            label.Text = message;
        }
    }

    private void UpdateGridCellAsync(DataGridViewRow row, string columnName, string value, int typeColor = 2, bool append = false)
    {
        if (append)
        {
            DataGridViewCell dataGridViewCell = row.Cells[columnName];
            dataGridViewCell.Value = dataGridViewCell.Value?.ToString() + value;
        }
        else
        {
            row.Cells[columnName].Value = value;
        }
        switch (typeColor)
        {
            case 0:
                row.Cells[columnName].Style.ForeColor = Color.Green;
                break;
            case 1:
                row.Cells[columnName].Style.ForeColor = Color.OrangeRed;
                break;
        }
    }

    private void CleanupResources()
    {
        _cts?.Dispose();
    }

    private async void xóaViaToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvVia.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            dtgvVia.Rows.Remove(row);
        }
        int index = 0;
        foreach (DataGridViewRow row2 in dtgvVia.Rows.Cast<DataGridViewRow>())
        {
            row2.Cells[0].Value = index++;
        }
        await UpdateLabelText(lblTotalCount, $"{dtgvVia.RowCount}");
    }

    private async void btnLoadBM_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvVia.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        int typeLoadBM = cbbTypeLoadBM.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        dtgvBM.Rows.Clear();
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    AccountDto accountDto = GetAccountDtoByRow(row);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, row, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    _ = exist.Account;
                    UpdateGridCellAsync(row, "cProcess", "Load All BM", 0);
                    List<BusinessManagermentDto> businessManages = await facebookHandler.LoadAllBM();
                    switch (typeLoadBM)
                    {
                        case 0:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 1);
                                return;
                            }
                            foreach (BusinessManagermentDto business5 in businessManages)
                            {
                                int index5 = 0;
                                dtgvBM.Invoke((Action)delegate
                                {
                                    index5 = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business5.BusinessInfo.BusinessId, accountDto.Uid, business5.BusinessInfo.BusinessName, business5.BusinessInfo.BusinessType, business5.BusinessInfo.BusinessVerified, business5.BusinessInfo.CanCreateAccount, "", "", business5.BusinessInfo.CreateTime, business5.BusinessInfo.StatusBusiness, "", business5.BusinessInfo.CountOwnerAdAccount);
                                });
                                if (business5.BusinessInfo.StatusBusiness == "BM Live")
                                {
                                    DataGridViewRow rowBM5 = dtgvBM.Rows[index5];
                                    rowBM5.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                        case 1:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 0);
                                return;
                            }
                            foreach (BusinessManagermentDto business7 in businessManages)
                            {
                                if (!(business7.BusinessInfo.StatusBusiness != "BM Live"))
                                {
                                    int index7 = 0;
                                    dtgvBM.Invoke((Action)delegate
                                    {
                                        index7 = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business7.BusinessInfo.BusinessId, accountDto.Uid, business7.BusinessInfo.BusinessName, business7.BusinessInfo.BusinessType, business7.BusinessInfo.BusinessVerified, business7.BusinessInfo.CanCreateAccount, "", "", business7.BusinessInfo.CreateTime, business7.BusinessInfo.StatusBusiness, "", business7.BusinessInfo.CountOwnerAdAccount);
                                    });
                                    if (business7.BusinessInfo.StatusBusiness == "BM Live")
                                    {
                                        DataGridViewRow rowBM7 = dtgvBM.Rows[index7];
                                        rowBM7.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                    }
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                        case 2:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 0);
                                return;
                            }
                            foreach (BusinessManagermentDto business3 in businessManages)
                            {
                                if (!(business3.BusinessInfo.StatusBusiness == "BM Live"))
                                {
                                    int index3 = 0;
                                    dtgvBM.Invoke((Action)delegate
                                    {
                                        index3 = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business3.BusinessInfo.BusinessId, accountDto.Uid, business3.BusinessInfo.BusinessName, business3.BusinessInfo.BusinessType, business3.BusinessInfo.BusinessVerified, business3.BusinessInfo.CanCreateAccount, "", "", business3.BusinessInfo.CreateTime, business3.BusinessInfo.StatusBusiness, "", business3.BusinessInfo.CountOwnerAdAccount);
                                    });
                                    if (business3.BusinessInfo.StatusBusiness == "BM Live")
                                    {
                                        DataGridViewRow rowBM3 = dtgvBM.Rows[index3];
                                        rowBM3.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                    }
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                        case 3:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 0);
                                return;
                            }
                            foreach (BusinessManagermentDto business6 in businessManages)
                            {
                                if (!(business6.BusinessInfo.StatusBusiness != "BM Live") && !(business6.BusinessInfo.BusinessType == "BM50"))
                                {
                                    int index6 = 0;
                                    dtgvBM.Invoke((Action)delegate
                                    {
                                        index6 = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business6.BusinessInfo.BusinessId, accountDto.Uid, business6.BusinessInfo.BusinessName, business6.BusinessInfo.BusinessType, business6.BusinessInfo.BusinessVerified, business6.BusinessInfo.CanCreateAccount, "", "", business6.BusinessInfo.CreateTime, business6.BusinessInfo.StatusBusiness, "", business6.BusinessInfo.CountOwnerAdAccount);
                                    });
                                    if (business6.BusinessInfo.StatusBusiness == "BM Live")
                                    {
                                        DataGridViewRow rowBM6 = dtgvBM.Rows[index6];
                                        rowBM6.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                    }
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                        case 4:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 0);
                                return;
                            }
                            foreach (BusinessManagermentDto business2 in businessManages)
                            {
                                if (!(business2.BusinessInfo.StatusBusiness == "BM Live") && !(business2.BusinessInfo.BusinessType == "BM50"))
                                {
                                    int index2 = 0;
                                    dtgvBM.Invoke((Action)delegate
                                    {
                                        index2 = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business2.BusinessInfo.BusinessId, accountDto.Uid, business2.BusinessInfo.BusinessName, business2.BusinessInfo.BusinessType, business2.BusinessInfo.BusinessVerified, business2.BusinessInfo.CanCreateAccount, "", "", business2.BusinessInfo.CreateTime, business2.BusinessInfo.StatusBusiness, "", business2.BusinessInfo.CountOwnerAdAccount);
                                    });
                                    if (business2.BusinessInfo.StatusBusiness == "BM Live")
                                    {
                                        DataGridViewRow rowBM2 = dtgvBM.Rows[index2];
                                        rowBM2.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                    }
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                        case 5:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 0);
                                return;
                            }
                            foreach (BusinessManagermentDto business4 in businessManages)
                            {
                                if (!(business4.BusinessInfo.BusinessType == "BM50"))
                                {
                                    int index4 = 0;
                                    dtgvBM.Invoke((Action)delegate
                                    {
                                        index4 = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business4.BusinessInfo.BusinessId, accountDto.Uid, business4.BusinessInfo.BusinessName, business4.BusinessInfo.BusinessType, business4.BusinessInfo.BusinessVerified, business4.BusinessInfo.CanCreateAccount, "", "", business4.BusinessInfo.CreateTime, business4.BusinessInfo.StatusBusiness, "", business4.BusinessInfo.CountOwnerAdAccount);
                                    });
                                    if (business4.BusinessInfo.StatusBusiness == "BM Live")
                                    {
                                        DataGridViewRow rowBM4 = dtgvBM.Rows[index4];
                                        rowBM4.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                    }
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                        case 6:
                            if (businessManages == null || businessManages.Count == 0)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Load BM lỗi!", 0);
                                return;
                            }
                            foreach (BusinessManagermentDto business in businessManages)
                            {
                                int index = 0;
                                dtgvBM.Invoke((Action)delegate
                                {
                                    index = dtgvBM.Rows.Add(dtgvBM.RowCount + 1, business.BusinessInfo.BusinessId, accountDto.Uid, business.BusinessInfo.BusinessName, business.BusinessInfo.BusinessType, business.BusinessInfo.BusinessVerified, business.BusinessInfo.CanCreateAccount, "", "", business.BusinessInfo.CreateTime, business.BusinessInfo.StatusBusiness, "", business.BusinessInfo.CountOwnerAdAccount);
                                });
                                if (business.BusinessInfo.StatusBusiness == "BM Live")
                                {
                                    DataGridViewRow rowBM = dtgvBM.Rows[index];
                                    rowBM.Cells["cStatusBM"].Style.ForeColor = Color.Green;
                                }
                            }
                            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
                            UpdateGridCellAsync(row, "cProcess", "Load BM xong", 0);
                            break;
                    }
                    UpdateGridCellAsync(row, "cProcess", $"Đã load {businessManages.Count} BM", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcess", "Load BM Lỗi", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process LoadBM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void dánIDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            string clipboard = Clipboard.GetText();
            if (string.IsNullOrEmpty(clipboard))
            {
                MessageBox.Show("Clipboard trống!");
                return;
            }
            dtgvBM.Rows.Clear();
            string[] raw = clipboard.Split('\n');
            string[] array = raw;
            foreach (string data in array)
            {
                if (!string.IsNullOrEmpty(data))
                {
                    string[] dataRaw = data.Split();
                    if (dataRaw.Length == 1 || string.IsNullOrEmpty(dataRaw[1]))
                    {
                        dtgvBM.Rows.Add(dtgvBM.RowCount, dataRaw[0].Trim(), dtgvVia.Rows[0].Cells["cUid"].Value?.ToString());
                    }
                    else
                    {
                        dtgvBM.Rows.Add(dtgvBM.RowCount, dataRaw[0].Trim(), dataRaw[1].Trim());
                    }
                }
            }
            await UpdateLabelText(lblCountTotalBM, $"{dtgvBM.RowCount}");
        }
        catch
        {
            MessageBox.Show("Data dán vào lỗi!", "Thông báo!");
        }
    }

    private async void loadTKQCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        dtgvTKQC.Rows.Clear();
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC", 0);
                    string businessId = row.Cells[1].Value?.ToString();
                    string uid = row.Cells[2].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist?.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    Task loadTaskType0 = Task.Run(async delegate
                    {
                        new List<AdAccountDto>();
                        string nextUrl = string.Empty;
                        do
                        {
                            List<AdAccountDto> adAccountDtos;
                            (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 0, nextUrl);
                            foreach (AdAccountDto adAccount in adAccountDtos)
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount.AccountId, uid, adAccount.AccountName, adAccount.AccountSpent, adAccount.AdAccountStatus, adAccount.Currency, adAccount.AccountLimit, adAccount.AccountThreshold, adAccount.AccountBalance, adAccount.SpendCap, adAccount.PaymentMethod, adAccount.TimeZone, adAccount.TypeAdAccount, adAccount.CreatedTime, adAccount.Owner, adAccount.BusinessCountryCode, adAccount.UserCount, adAccount.PartnerCount, adAccount.CampaignCount, adAccount.BusinessId);
                                });
                            }
                        }
                        while (!string.IsNullOrEmpty(nextUrl));
                    });
                    Task loadTaskType1 = Task.Run(async delegate
                    {
                        new List<AdAccountDto>();
                        string nextUrl = string.Empty;
                        do
                        {
                            List<AdAccountDto> adAccountDtos;
                            (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 1, nextUrl);
                            foreach (AdAccountDto adAccount in adAccountDtos)
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount.AccountId, uid, adAccount.AccountName, adAccount.AccountSpent, adAccount.AdAccountStatus, adAccount.Currency, adAccount.AccountLimit, adAccount.AccountThreshold, adAccount.AccountBalance, adAccount.SpendCap, adAccount.PaymentMethod, adAccount.TimeZone, adAccount.TypeAdAccount, adAccount.CreatedTime, adAccount.Owner, adAccount.BusinessCountryCode, adAccount.UserCount, adAccount.PartnerCount, adAccount.CampaignCount, adAccount.BusinessId);
                                });
                            }
                        }
                        while (!string.IsNullOrEmpty(nextUrl));
                    });
                    await Task.WhenAll(loadTaskType0, loadTaskType1);
                    await UpdateLabelText(lblTotalCountTKQC, $"{dtgvTKQC.RowCount}");
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Load TKQC: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void btnLoadTKQC_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvVia.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeLoadBM.SelectedIndex;
        dtgvTKQC.Rows.Clear();
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcess", "Load TKQC", 0);
                    AccountDto accountDto = GetAccountDtoByRow(row);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, row, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    row.Cells[1].Value.ToString();
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    new List<AdAccountDto>();
                    _ = string.Empty;
                    string nextUrl;
                    do
                    {
                        List<AdAccountDto> clientAdAccountDtos;
                        (clientAdAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountPersonV2();
                        if (clientAdAccountDtos == null && !clientAdAccountDtos.Any())
                        {
                            UpdateGridCellAsync(row, "cProcess", "Load TKQC lỗi", 1);
                            return;
                        }
                        foreach (AdAccountDto adAccount in clientAdAccountDtos)
                        {
                            int index = 0;
                            dtgvTKQC.Invoke((Action)delegate
                            {
                                index = dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount.AccountId, accountDto.Uid, adAccount.AccountName, adAccount.AccountSpent, adAccount.AdAccountStatus, adAccount.Currency, adAccount.AccountLimit, adAccount.AccountThreshold, adAccount.AccountBalance, adAccount.SpendCap, adAccount.PaymentMethod, adAccount.TimeZone, adAccount.TypeAdAccount, adAccount.CreatedTime, adAccount.Owner, adAccount.BusinessCountryCode, adAccount.UserCount, adAccount.PartnerCount, adAccount.CampaignCount, adAccount.BusinessId);
                            });
                            DataGridViewRow rowTKQC = dtgvTKQC.Rows[index];
                            switch (adAccount.TypeStatus)
                            {
                                case 0:
                                    rowTKQC.Cells[5].Style.ForeColor = Color.Blue;
                                    break;
                                case 1:
                                    rowTKQC.Cells[5].Style.ForeColor = Color.Green;
                                    break;
                                case 2:
                                    rowTKQC.Cells[5].Style.ForeColor = Color.OrangeRed;
                                    break;
                                case 3:
                                    rowTKQC.Cells[5].Style.ForeColor = Color.YellowGreen;
                                    break;
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    await UpdateLabelText(lblTotalCountTKQC, $"{dtgvTKQC.RowCount}");
                    UpdateGridCellAsync(row, "cProcess", "Load TKQC Xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Load TKQC: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkInfoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                string uid = row.Cells[2].Value.ToString();
                string businessId = row.Cells[1].Value.ToString();
                try
                {
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist?.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    BusinessManagermentDto businessManages = await facebookHandler.CheckBusinessInfomation(businessId);
                    if (businessManages == null)
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Load BM lỗi!", 0);
                    }
                    else
                    {
                        FacebookHandlerContainer.Instance.UpdateFacebookHandlerProfile(exist);
                        string businessUserId = string.Join(Environment.NewLine, businessManages.BusinessUsers.Select((BusinessUserDto b) => b.UserName.Split()[0]));
                        dtgvBM.Invoke((Action)delegate
                        {
                            row.Cells[3].Value = businessManages.BusinessInfo.BusinessName;
                            row.Cells[4].Value = businessManages.BusinessInfo.BusinessType;
                            row.Cells[5].Value = businessManages.BusinessInfo.BusinessVerified;
                            row.Cells[6].Value = businessManages.BusinessInfo.CanCreateAccount;
                            row.Cells[7].Value = businessManages.BusinessUsers.Count;
                            row.Cells[8].Value = "";
                            row.Cells[9].Value = businessManages.BusinessInfo.CreateTime;
                            row.Cells[10].Value = businessManages.BusinessInfo.StatusBusiness;
                            row.Cells[11].Value = "";
                            row.Cells[12].Value = businessManages.BusinessInfo.CountOwnerAdAccount;
                            row.Cells[13].Value = businessUserId;
                        });
                        await Task.Run(async delegate
                        {
                            DataGridViewCell dataGridViewCell = row.Cells[8];
                            dataGridViewCell.Value = await facebookHandler.CheckLimitBusiness(businessManages.BusinessInfo.BusinessId);
                        });
                        if (businessManages.BusinessInfo.StatusBusiness == "BM Die")
                        {
                            row.Cells[10].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            row.Cells[10].Style.ForeColor = Color.DarkGreen;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Check Info BM for UID " + uid + ": " + ex3.Message + "\nStackTrace: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private int FindRowIndexByUid(DataGridView dgv, string uid)
    {
        List<DataGridViewRow> rows = dgv.Rows.Cast<DataGridViewRow>().ToList();
        foreach (DataGridViewRow row in rows)
        {
            DataGridViewCell uidRow = row.Cells.Cast<DataGridViewCell>().FirstOrDefault((DataGridViewCell x) => x.Value?.ToString() == uid);
            if (uidRow != null)
            {
                string uidVia = uidRow.Value.ToString();
                if (uidVia == uid)
                {
                    return row.Index;
                }
            }
        }
        return -1;
    }

    private List<int> FindListRowIndex(DataGridView dgv, string uid)
    {
        List<int> lstIndex = new List<int>();
        List<DataGridViewRow> rows = dgv.Rows.Cast<DataGridViewRow>().ToList();
        foreach (DataGridViewRow row in rows)
        {
            DataGridViewCell uidRow = row.Cells.Cast<DataGridViewCell>().FirstOrDefault((DataGridViewCell x) => x.Value?.ToString() == uid);
            if (uidRow != null)
            {
                lstIndex.Add(row.Index);
            }
        }
        return lstIndex;
    }

    private async void outBMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeLoadBM.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Out BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid.Trim());
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string businessId = row.Cells[1].Value.ToString();
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            if (await facebookHandler.OutBusinessManage(businessId))
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Out BM thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Out BM thất bại", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnBackUp_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        int typeMail = cbbTypeMail.SelectedIndex;
        _ = tbMailPass.Text;
        _ = cbbTypeRole.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "BackUp BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        MailHelper mailHelper = new MailHelper();
                        string mail = string.Empty;
                        switch (typeMail)
                        {
                            case 0:
                                mail = await mailHelper.GetMailMoakt();
                                break;
                            case 1:
                                mail = "tqi" + Common.CreateRandomString(7) + "@mailngon.top";
                                break;
                            case 2:
                                mail = "tqi" + Common.CreateRandomString(7) + "@fviainboxes.com";
                                break;
                            case 3:
                                Invoke((Action)delegate
                                {
                                    mail = tbMailPass.Text;
                                });
                                break;
                        }
                        FacebookHandler.StatusShareBM statusInvite = await facebookHandler.ShareBMRequest(businessId, mail, antispam: false);
                        if (statusInvite != FacebookHandler.StatusShareBM.Success)
                        {
                            UpdateGridCellAsync(row, "cProcessBM", $"Share BM thất bại: {statusInvite}", 1);
                        }
                        else
                        {
                            DataGridViewHelper.SetStatusDataGridViewWithWait(row, "cProcessBM", 5);
                            UpdateGridCellAsync(row, "cProcessBM", "Lấy link BM", 0);
                            string linkBM = string.Empty;
                            switch (typeMail)
                            {
                                case 0:
                                    linkBM = await mailHelper.ReadInboxMoakt();
                                    break;
                                case 1:
                                    linkBM = await MailHelper.GetLinkMailNgon(mail.Split('@')[0]);
                                    break;
                                case 2:
                                    linkBM = await MailHelper.GetLinkFViaInboxes(mail.Split('@')[0]);
                                    break;
                            }
                            rtbLinkBM.Invoke((Action)delegate
                            {
                                RichTextBox richTextBox = rtbLinkBM;
                                richTextBox.Text = richTextBox.Text + linkBM + Environment.NewLine;
                            });
                            UpdateGridCellAsync(row, "cProcessBM", linkBM, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private void rtbLinkBM_TextChanged(object sender, EventArgs e)
    {
        int linkCount = rtbLinkBM.Lines.Count((string x) => x.Contains("https://business.facebook.com/invitation/?"));
        lblCountLinkBM.Text = $"{linkCount}";
    }

    private async void btnCreateAdAccount_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        int countAdAccount = (int)nudCountNumAdAccount.Value;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string currency = string.Empty;
                        string timeZone = string.Empty;
                        string region = string.Empty;
                        string partner = string.Empty;
                        string baseName = string.Empty;
                        Invoke((Action)delegate
                        {
                            currency = cbbCurrencyBM.Text.Split('(')[1].Replace(")", "");
                            timeZone = cbbTimeZoneBM.SelectedValue?.ToString();
                            region = tbRegion.Text;
                            partner = tbPartner.Text;
                            baseName = tbNameBM.Text;
                        });
                        for (int i = 0; i < countAdAccount; i++)
                        {
                            string name = $"{baseName} {i + 1}";
                            var (statusCreateAdAccount, message) = await facebookHandler.CreateAdAccountInBMVer2(businessId, name, currency, timeZone, partner);
                            if (statusCreateAdAccount != FacebookHandler.StatusCreateBM.Success)
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC thất bại", 1);
                                return;
                            }
                            UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC thành công: " + message, 0);
                        }
                        UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC thành công", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
        try
        {
            string clipboard = Clipboard.GetText();
            if (string.IsNullOrEmpty(clipboard))
            {
                MessageBox.Show("Clipboard trống!");
                return;
            }
            dtgvTKQC.Rows.Clear();
            string[] raw = clipboard.Split('\n');
            string[] array = raw;
            foreach (string data in array)
            {
                if (!string.IsNullOrEmpty(data))
                {
                    string[] dataRaw = data.Split();
                    if (dataRaw.Length == 1 || string.IsNullOrEmpty(dataRaw[1]))
                    {
                        dtgvTKQC.Rows.Add(dtgvTKQC.RowCount, dataRaw[0].Trim(), dtgvVia.Rows[0].Cells["cUid"].Value?.ToString());
                    }
                    else
                    {
                        dtgvTKQC.Rows.Add(dtgvTKQC.RowCount, dataRaw[0].Trim(), dataRaw[1].Trim());
                    }
                }
            }
        }
        catch
        {
            MessageBox.Show("Data dán vào lỗi!", "Thông báo!");
        }
    }

    private async void toolStripMenuItem2_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow row in dtgvTKQC.Rows
                                              where row.Selected || row.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select row).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        var rows = from DataGridViewRow row in selectedRows
                   where !row.IsNewRow
                   select new
                   {
                       ID = row.Cells[1].Value?.ToString(),
                       UID = row.Cells[2].Value?.ToString()
                   } into item
                   where item.UID != null
                   select item;
        var groupedData = from item in rows
                          group item by item.UID into g
                          select new
                          {
                              GroupKey = g.Key,
                              Values = g.Select(item => item.ID).ToList()
                          };
        foreach (var group in groupedData)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = group.GroupKey.Trim();
                    List<int> lstRowIndex = FindListRowIndex(dtgvVia, uid);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                    if (exist == null)
                    {
                        foreach (int item in lstRowIndex)
                        {
                            DataGridViewRow row = dtgvVia.Rows[item];
                            UpdateGridCellAsync(row, "cProcess", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                    }
                    else
                    {
                        FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                        facebookHandler.LoginCookie();
                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        List<AdAccountDto> adAccounts = await facebookHandler.BatchCheckMultiAdAccount(group.Values);
                        if (adAccounts == null || adAccounts.Count == 0)
                        {
                            foreach (int item2 in lstRowIndex)
                            {
                                DataGridViewRow row2 = dtgvVia.Rows[item2];
                                UpdateGridCellAsync(row2, "cProcess", "Check TKQC lỗi!", 0);
                            }
                        }
                        else
                        {
                            stopWatch.Start();
                            foreach (AdAccountDto adAccount in adAccounts)
                            {
                                int index = FindRowIndexByUid(dtgvTKQC, adAccount.AccountId);
                                if (index != -1)
                                {
                                    DataGridViewRow row3 = dtgvTKQC.Rows[index];
                                    row3.Cells[3].Value = adAccount.AccountName;
                                    row3.Cells[4].Value = adAccount.AccountSpent;
                                    row3.Cells[5].Value = adAccount.AdAccountStatus;
                                    row3.Cells[6].Value = adAccount.Currency;
                                    row3.Cells[7].Value = adAccount.AccountLimit;
                                    row3.Cells[8].Value = adAccount.AccountThreshold;
                                    row3.Cells[9].Value = adAccount.AccountBalance;
                                    row3.Cells[10].Value = adAccount.SpendCap;
                                    row3.Cells[11].Value = adAccount.PaymentMethod;
                                    row3.Cells[12].Value = adAccount.TimeZone;
                                    row3.Cells[13].Value = adAccount.TypeAdAccount;
                                    row3.Cells[14].Value = adAccount.CreatedTime;
                                    row3.Cells[15].Value = adAccount.Owner;
                                    row3.Cells[16].Value = adAccount.BusinessCountryCode;
                                    row3.Cells[17].Value = adAccount.Users;
                                    row3.Cells[18].Value = adAccount.Partners;
                                    row3.Cells[19].Value = adAccount.CampaignCount;
                                    row3.Cells[20].Value = adAccount.BusinessId;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Load TKQC: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void thoátTKQCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành xóa TKQC khỏi BM", 0);
                        string adAccount = row.Cells[1].Value?.ToString();
                        string businessId = row.Cells["cBussinessId"].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.RemoveAdAccount(adAccount, businessId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Out thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Out thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    UpdateGridCellAsync(row, "cProcessTKQC", ex2.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void xóaQTVToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Processing....", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccount = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        UpdateGridCellAsync(row, "cProcessTKQC", "Đang xóa QTV và đối tác", 0);
                        var (statusOutTKQC, successCount, failedCount) = await facebookHandler.RemoveAllQTV(adAccount);
                        if (statusOutTKQC)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", $"Xóa thành công: {successCount} - Thất bại: {failedCount}", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", $"Xóa thành công: {successCount} - Thất bại: {failedCount}", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void đóngTKQCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.CheckCookieStatus() != FacebookHandler.StatusCookie.Live)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Cookie die. Vui lòng login lại!", 1);
                        }
                        else if (await facebookHandler.CloseAdAccountInBM(adAccountId) == EnumStatus.CloseStatus.Success)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Đóng thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Đóng thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void mởTKQCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.ReActiveAdAccount(adAccountId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Mở thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Mở thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private void tsmPasteBusinessId_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        string clipboard = Clipboard.GetText();
        if (string.IsNullOrEmpty(clipboard))
        {
            MessageBox.Show("Copy data id BM trước khi dán!");
            return;
        }
        int index = 0;
        string[] lines = clipboard.Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (index >= lines.Count())
            {
                break;
            }
            row.Cells["cBussinessId"].Value = lines[index];
            if (lines.Count() > 1)
            {
                index++;
            }
        }
    }

    private async void btnAssignPartner_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        int typePartner = cbbPermitTask.SelectedIndex;
        List<string> partnerIds = tbPartnerId.Lines.ToList();
        if (partnerIds.Count == 0)
        {
            MessageBox.Show("Nhập ID đối tác trước khi chạy chức năng này!", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        string businessId = row.Cells["cBussinessId"].Value?.ToString();
                        if (string.IsNullOrEmpty(businessId))
                        {
                            businessId = row.Cells["cOwnerTKQC"].Value?.ToString();
                            if (string.IsNullOrEmpty(businessId))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Chọn chức năng dán BM trước khi chạy!", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        foreach (string partnerId in partnerIds)
                        {
                            if (await facebookHandler.AssignPartner(adAccountId, businessId, partnerId, typePartner))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", partnerId + " Share " + businessId + " Ok\n", 0, append: true);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", partnerId + " Share " + businessId + " Failed\n", 1, append: true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnRemovePartner_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbPermitTask.SelectedIndex;
        string partnerId = tbPartnerId.Text;
        if (string.IsNullOrEmpty(partnerId))
        {
            MessageBox.Show("Nhập ID đối tác trước khi chạy chức năng này!", "Thông báo!!!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        string businessId = row.Cells["cBussinessId"].Value?.ToString();
                        if (string.IsNullOrEmpty(businessId))
                        {
                            businessId = row.Cells["cOwnerTKQC"].Value?.ToString();
                            if (string.IsNullOrEmpty(businessId))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Chọn chức năng dán BM trước khi chạy!", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.DeletePartner(adAccountId, businessId, partnerId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", businessId + " Xóa " + partnerId + " Ok", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", businessId + " Xóa " + partnerId + " Failed", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void loadUserToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        dtgvUser.Rows.Clear();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Load User", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        _ = exist.Account.BusinessManagerments;
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string businessId = row.Cells[1].Value.ToString();
                        List<BusinessUserDto> users = await facebookHandler.LoadUser(businessId);
                        if (users == null && users.Count == 0)
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Load UserBM lỗi!", 1);
                        }
                        else
                        {
                            foreach (BusinessUserDto user in users)
                            {
                                dtgvUser.Invoke((Action)delegate
                                {
                                    dtgvUser.Rows.Add(dtgvUser.RowCount + 1, uid, user.UserId, businessId, user.UserEmail, user.UserName, user.UserRole);
                                });
                            }
                            UpdateGridCellAsync(row, "cProcessBM", "Load User Xong", 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", ex3.Message, 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void xóaQuảnTrịToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvUser.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cStatusUser", "Đang xóa....", 0);
                    string uid = row.Cells[1].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cStatusUser", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string userId = row.Cells[2].Value.ToString();
                        string businessId = row.Cells[3].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.DeleteUser(userId, businessId))
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Delete thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Delete thất bại", 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error DeleteUser: " + ex3.StackTrace + "\n";
                    });
                    UpdateGridCellAsync(row, "cStatusUser", ex3.Message ?? "", 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void xóaQuảnTrịTrước7NgàyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvUser.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cStatusUser", "Xóa ADMIN...", 0);
                    string uid = row.Cells[1].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cStatusUser", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid.Trim());
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string userId = row.Cells[2].Value.ToString();
                            string businessId = row.Cells[3].Value.ToString();
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            var (statusDeleteUser, message) = await facebookHandler.DeleteUserBefore7Days(userId, businessId);
                            if (statusDeleteUser)
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Delete thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Delete thất bại-" + message, 0);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error DeleteUser: " + ex3.StackTrace + "\n";
                    });
                    UpdateGridCellAsync(row, "cStatusUser", ex3.Message ?? "", 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private void xóaLờiMờiToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private async void hạQuyềnToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvUser.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cStatusUser", "BackUp BM", 0);
                    string uid = row.Cells[1].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cStatusUser", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid.Trim());
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string userId = row.Cells[2].Value.ToString();
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            if (await facebookHandler.CheckCookieStatus() != FacebookHandler.StatusCookie.Live)
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Cookie die! Vui lòng chọn login lại", 1);
                            }
                            else if (await facebookHandler.DownTask(userId))
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Hạ quyền thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Hạ quyền thất bại", 0);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Hạ quyền: " + ex3.StackTrace + "\n";
                    });
                    UpdateGridCellAsync(row, "cStatusUser", ex3.Message ?? "", 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnChangeNameBM_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        string baseName = tbBusinessName.Text;
        int index = 0;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "BackUp BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.CheckCookieStatus() != FacebookHandler.StatusCookie.Live)
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Cookie die! Vui lòng chọn login lại", 1);
                        }
                        else
                        {
                            string businessName = $"{baseName} {Interlocked.Increment(ref index)}";
                            if (await facebookHandler.ChangeNameBM(businessId, businessName))
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Change thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Change thất bại", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void thoátTKQCToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string adAccount = row.Cells[1].Value?.ToString();
                            row.Cells["cBussinessId"].Value?.ToString();
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            if (await facebookHandler.CheckCookieStatus() != FacebookHandler.StatusCookie.Live)
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Cookie die. Vui lòng login lại!", 1);
                            }
                            else if (await facebookHandler.OutAdAccount(adAccount))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Out thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Out thất bại", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void loadDieToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        dtgvTKQC.Rows.Clear();
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    new List<AdAccountDto>();
                    string nextUrl = string.Empty;
                    List<AdAccountDto> adAccountDtos;
                    do
                    {
                        (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 0, 0, nextUrl);
                        foreach (AdAccountDto adAccount in adAccountDtos)
                        {
                            if (adAccount.AdAccountStatus.Contains("Live"))
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount.AccountId, uid, adAccount.AccountName, adAccount.AccountSpent, adAccount.AdAccountStatus, adAccount.Currency, adAccount.AccountLimit, adAccount.AccountThreshold, adAccount.AccountBalance, adAccount.SpendCap, adAccount.PaymentMethod, adAccount.TimeZone, adAccount.TypeAdAccount, adAccount.CreatedTime, adAccount.Owner, adAccount.BusinessCountryCode, adAccount.UserCount, adAccount.PartnerCount, adAccount.CampaignCount);
                                });
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    adAccountDtos.Clear();
                    nextUrl = string.Empty;
                    do
                    {
                        (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 1, 0, nextUrl);
                        foreach (AdAccountDto adAccount2 in adAccountDtos)
                        {
                            if (adAccount2.AdAccountStatus.Contains("Live"))
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount2.AccountId, uid, adAccount2.AccountName, adAccount2.AccountSpent, adAccount2.AdAccountStatus, adAccount2.Currency, adAccount2.AccountLimit, adAccount2.AccountThreshold, adAccount2.AccountBalance, adAccount2.SpendCap, adAccount2.PaymentMethod, adAccount2.TimeZone, adAccount2.TypeAdAccount, adAccount2.CreatedTime, adAccount2.Owner, adAccount2.BusinessCountryCode, adAccount2.UserCount, adAccount2.PartnerCount, adAccount2.CampaignCount);
                                });
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    await UpdateLabelText(lblTotalCountTKQC, $"{dtgvTKQC.RowCount}");
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Load TKQC: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void loadLiveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        dtgvTKQC.Rows.Clear();
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    new List<AdAccountDto>();
                    string nextUrl = string.Empty;
                    List<AdAccountDto> adAccountDtos;
                    do
                    {
                        (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 0, 1, nextUrl);
                        foreach (AdAccountDto adAccount in adAccountDtos)
                        {
                            if (adAccount.AdAccountStatus.Contains("Live"))
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount.AccountId, uid, adAccount.AccountName, adAccount.AccountSpent, adAccount.AdAccountStatus, adAccount.Currency, adAccount.AccountLimit, adAccount.AccountThreshold, adAccount.AccountBalance, adAccount.SpendCap, adAccount.PaymentMethod, adAccount.TimeZone, adAccount.TypeAdAccount, adAccount.CreatedTime, adAccount.Owner, adAccount.BusinessCountryCode, adAccount.UserCount, adAccount.PartnerCount, adAccount.CampaignCount);
                                });
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    adAccountDtos.Clear();
                    nextUrl = string.Empty;
                    do
                    {
                        (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 1, 1, nextUrl);
                        foreach (AdAccountDto adAccount2 in adAccountDtos)
                        {
                            if (adAccount2.AdAccountStatus.Contains("Live"))
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount2.AccountId, uid, adAccount2.AccountName, adAccount2.AccountSpent, adAccount2.AdAccountStatus, adAccount2.Currency, adAccount2.AccountLimit, adAccount2.AccountThreshold, adAccount2.AccountBalance, adAccount2.SpendCap, adAccount2.PaymentMethod, adAccount2.TimeZone, adAccount2.TypeAdAccount, adAccount2.CreatedTime, adAccount2.Owner, adAccount2.BusinessCountryCode, adAccount2.UserCount, adAccount2.PartnerCount, adAccount2.CampaignCount);
                                });
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    await UpdateLabelText(lblTotalCountTKQC, $"{dtgvTKQC.RowCount}");
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Load TKQC: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void kíchAppToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang kích App...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    await exist.FacebookHandler.KichApp3(businessId);
                    await exist.FacebookHandler.CreateWA(businessId);
                    UpdateGridCellAsync(row, "cProcessBM", "Kích App xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Kích App Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private void dtgvBM_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
        {
            return;
        }
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count <= 0)
        {
            return;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            dtgvBM.Rows.Remove(row);
        }
    }

    private void dtgvTKQC_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
        {
            return;
        }
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count <= 0)
        {
            return;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            dtgvTKQC.Rows.Remove(row);
        }
    }

    private void dtgvUser_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
        {
            return;
        }
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvUser.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count <= 0)
        {
            return;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            dtgvUser.Rows.Remove(row);
        }
    }

    private async void gánQuyềnAddThẻToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            row.Cells[1].Value?.ToString();
                            string businessId = row.Cells["cBussinessId"].Value?.ToString();
                            FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                            facebookHandler.LoginCookie();
                            string businessUserId = await facebookHandler.GetMyBusinessUserId(businessId);
                            if (string.IsNullOrEmpty(businessUserId))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Không get được USERID trong BM!", 1);
                            }
                            else if (await facebookHandler.AssignFinanceRole(businessUserId))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Gán thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Gán thất bại", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnBuildBM_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string businessId = tbBMID.Text;
        if (string.IsNullOrEmpty(businessId))
        {
            MessageBox.Show("Vui lòng nhập BMID!");
            return;
        }
        new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Processing...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        return;
                    }
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist?.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    string adAccount = row.Cells[1].Value?.ToString();
                    FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                    facebookHandler.LoginCookie();
                    if (await facebookHandler.AddAdAccountIntoBM(businessId, adAccount))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Nhét thành công", 0);
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Nhét thất bại", 1);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(1.75));
                }
            }));
            await Task.Delay(TimeSpan.FromSeconds(1.0));
        }
    }

    private async void mởBMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow row in dtgvBM.Rows
                                              where row.Selected || row.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select row).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        object lockObject = new object();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        var rows = from DataGridViewRow row in selectedRows
                   where !row.IsNewRow
                   select new
                   {
                       ID = row.Cells[1].Value?.ToString(),
                       UID = row.Cells[2].Value?.ToString()
                   } into item
                   where item.UID != null
                   select item;
        var groupedData = from item in rows
                          group item by item.UID into g
                          select new
                          {
                              GroupKey = g.Key,
                              Values = g.Select(item => item.ID).ToList()
                          };
        foreach (var group in groupedData)
        {
            semaphore.Wait();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = group.GroupKey;
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    ChromeHandler chromeHandler = exist.ChromeHandler;
                    lock (lockObject)
                    {
                        if (chromeHandler == null)
                        {
                            ChromeService chromeService = new ChromeService();
                            (ChromeService.StatusOpenChrome, string) tuple = chromeService.OpenChrome();
                            var (statusOpen, _) = tuple;
                            _ = tuple.Item2;
                            if (statusOpen == ChromeService.StatusOpenChrome.Closed)
                            {
                                List<int> lstIndex = FindListRowIndex(dtgvBM, uid);
                                foreach (int index in lstIndex)
                                {
                                    DataGridViewRow row = dtgvBM.Rows[index];
                                    UpdateGridCellAsync(row, "cProcessBM", "Vui lòng tắt chrome profile trước khi mở!", 1);
                                }
                                return;
                            }
                            chromeHandler = new ChromeHandler(chromeService, exist.Account);
                            if (!chromeHandler.LoginFacebook())
                            {
                                List<int> lstIndex2 = FindListRowIndex(dtgvBM, uid);
                                foreach (int index2 in lstIndex2)
                                {
                                    DataGridViewRow row2 = dtgvBM.Rows[index2];
                                    UpdateGridCellAsync(row2, "cProcessBM", "Đăng nhập thất bại!", 1);
                                }
                                return;
                            }
                        }
                        else if (!chromeHandler.IsLive())
                        {
                            ChromeService chromeService2 = new ChromeService();
                            (ChromeService.StatusOpenChrome, string) tuple = chromeService2.OpenChrome();
                            var (statusOpen2, _) = tuple;
                            _ = tuple.Item2;
                            if (statusOpen2 == ChromeService.StatusOpenChrome.Closed)
                            {
                                List<int> lstIndex3 = FindListRowIndex(dtgvBM, uid);
                                foreach (int index3 in lstIndex3)
                                {
                                    DataGridViewRow row3 = dtgvBM.Rows[index3];
                                    UpdateGridCellAsync(row3, "cProcessBM", "Vui lòng tắt chrome profile trước khi mở!", 1);
                                }
                                return;
                            }
                            chromeHandler = new ChromeHandler(chromeService2, exist.Account);
                            if (!chromeHandler.LoginFacebook())
                            {
                                List<int> lstIndex4 = FindListRowIndex(dtgvBM, uid);
                                foreach (int index4 in lstIndex4)
                                {
                                    DataGridViewRow row4 = dtgvBM.Rows[index4];
                                    UpdateGridCellAsync(row4, "cProcessBM", "Đăng nhập thất bại!", 1);
                                }
                                return;
                            }
                        }
                        exist.ChromeHandler = chromeHandler;
                        FacebookHandlerContainer.Instance.UpdateFacebookHandlerProfile(exist);
                    }
                    foreach (string businessId in group.Values)
                    {
                        int index5 = FindRowIndexByUid(dtgvBM, businessId);
                        if (index5 == -1)
                        {
                            return;
                        }
                        DataGridViewRow row5 = dtgvBM.Rows[index5];
                        UpdateGridCellAsync(row5, "cProcessBM", "Đang mở chrome...", 0);
                        chromeHandler.OpenNewTab("https://business.facebook.com/latest/settings/business_users/?business_id=" + businessId, switchToLastTab: true);
                        await Task.Delay(TimeSpan.FromSeconds(2.0));
                        chromeHandler.RemoveBlock();
                        UpdateGridCellAsync(row5, "cProcessBM", "Mở chrome xong", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Mở chrome: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void btnOutBM_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeLoadBM.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Out BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.OutBusinessManage(businessId))
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Out BM thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Out BM thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void xóaLờiMờiToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvUser.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeLoadBM.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cStatusUser", "Xóa lời mời BM", 0);
                    string uid = row.Cells[1].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cStatusUser", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid.Trim());
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            row.Cells[3].Value.ToString();
                            string requestId = row.Cells[2].Value.ToString();
                            if (await facebookHandler.DiscardInvite(requestId))
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Hủy lời mời thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cStatusUser", "Hủy lời mời thất bại", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Hủy lời mời BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkBMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeLoadBM.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Check BM Die", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string message = await facebookHandler.CheckRetrictBM(businessId);
                        UpdateGridCellAsync(row, "cProcessBM", message, 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Check lỗi!", 0);
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnChangeEmail_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string email = tbMailBM.Text;
        if (string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Vui lòng nhập email để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đổi mail BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string businessId = row.Cells[1].Value.ToString();
                        string businessUserId = await facebookHandler.GetBusinessUserId(businessId);
                        if (string.IsNullOrEmpty(businessUserId))
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Không get được USERID trong BM!", 1);
                        }
                        else if (await facebookHandler.AddNewMail(businessUserId, businessId, email))
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Add mail thành công=> " + email, 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Add mail thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Add Mail BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnSetGHCT_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        string spendCap = tbGHCT.Text;
        if (string.IsNullOrEmpty(spendCap))
        {
            MessageBox.Show("Vui lòng chọn nhập GHCT để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string adAccount = row.Cells[1].Value?.ToString();
                            string currency = row.Cells["cCurrencyTKQC"].Value?.ToString();
                            if (string.IsNullOrEmpty(currency))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Check TKQC trước khi set GHCT", 1);
                            }
                            else
                            {
                                FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                                facebookHandler.LoginCookie();
                                if (await facebookHandler.SetSpendCap(adAccount, spendCap, currency))
                                {
                                    UpdateGridCellAsync(row, "cProcessTKQC", "Set GHCT thành công", 0);
                                }
                                else
                                {
                                    UpdateGridCellAsync(row, "cProcessTKQC", "Set GHCT thất bại", 1);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkCampaignToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string adAccount = row.Cells[1].Value?.ToString();
                            FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                            facebookHandler.LoginCookie();
                            string messageCampaign = await facebookHandler.CheckCampaign(adAccount);
                            UpdateGridCellAsync(row, "cCampaignTKQC", messageCampaign, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkBillToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        }
                        else
                        {
                            string adAccount = row.Cells[1].Value?.ToString();
                            FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                            facebookHandler.LoginCookie();
                            if (await facebookHandler.CheckIsPay(adAccount))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Đã có bill", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Chưa có bill!", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void payToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccount = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                        facebookHandler.LoginCookie();
                        if (await facebookHandler.Pay(adAccount))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Pay thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Pay thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private void btnJoinBM_Click(object sender, EventArgs e)
    {
        List<string> lstLinkBM = tbLinkBM.Lines.ToList();
        if (lstLinkBM.Count == 0)
        {
            MessageBox.Show("Nhập danh sách Link BM");
            return;
        }
        List<string> lstLinkTemp = lstLinkBM;
        Task.Run(async delegate
        {
            DataGridViewRow row = dtgvVia.Rows[0];
            AccountDto accountDto = GetAccountDtoByRow(row);
            int countSuccess = 0;
            int countFailure = 0;
            foreach (string link in lstLinkBM)
            {
                FacebookHandler facebookHandler = new FacebookHandler(accountDto, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                facebookHandler.LoginCookie();
                (FacebookHandler.StatusJoinBM, string) tuple = await facebookHandler.JoinByLinkInvite(link, accountDto.Uid, "TQI");
                var (status, _) = tuple;
                _ = tuple.Item2;
                if (status != FacebookHandler.StatusJoinBM.Success)
                {
                    countSuccess++;
                    lstLinkTemp.Remove(link);
                    tbLinkBM.Invoke((Action)delegate
                    {
                        tbLinkBM.Lines = lstLinkBM.ToArray();
                    });
                }
                else
                {
                    countFailure++;
                }
                UpdateGridCellAsync(row, "cProcess", $"Nhận thành công: {countSuccess} - thất bại: {countFailure}", 0);
            }
        });
    }

    private async void btnShareTKQC_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string[] lstUid = tbLstUidVia.Lines;
        if (lstUid.Count() == 0)
        {
            MessageBox.Show("Vui lòng nhập Uid để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string adAccountId = row.Cells[1].Value?.ToString();
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string[] array = lstUid;
                        foreach (string uidVia in array)
                        {
                            if (await exist.FacebookHandler.ShareTKQC(adAccountId, uidVia))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Share thành công: " + uidVia, 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Share thất bại: " + uidVia, 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkInfoBMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string email = tbMailBM.Text;
        if (string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Vui lòng nhập email để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Check Info BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string businessId = row.Cells[1].Value.ToString();
                        string limit = await facebookHandler.CheckLimitBusiness(businessId);
                        dtgvBM.Invoke((Action)delegate
                        {
                            row.Cells[8].Value = limit;
                        });
                        UpdateGridCellAsync(row, "cProcessBM", "Check Info BM Xong", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Add Mail BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkLimitBMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string email = tbMailBM.Text;
        if (string.IsNullOrEmpty(email))
        {
            MessageBox.Show("Vui lòng nhập email để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Check Limit BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string businessId = row.Cells[1].Value.ToString();
                        var (limit, ownedAccountCount) = await facebookHandler.CheckLimitBM(businessId);
                        dtgvBM.Invoke((Action)delegate
                        {
                            row.Cells[11].Value = limit;
                            row.Cells[12].Value = ownedAccountCount;
                        });
                        UpdateGridCellAsync(row, "cProcessBM", "Xong", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Check Limit BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnRemoveGHCT_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string spendCap = tbGHCT.Text;
        if (string.IsNullOrEmpty(spendCap))
        {
            MessageBox.Show("Vui lòng chọn nhập GHCT để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        string adAccount = row.Cells[1].Value?.ToString();
                        string currency = row.Cells["cCurrencyTKQC"].Value?.ToString();
                        if (string.IsNullOrEmpty(currency))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Check TKQC trước khi set GHCT", 1);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành gỡ GHCT", 0);
                            int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                            DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                            AccountDto accountDto = GetAccountDtoByRow(rowVia);
                            FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist == null || exist.FacebookHandler == null)
                            {
                                await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                                exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                                if (exist?.FacebookHandler == null)
                                {
                                    UpdateGridCellAsync(row, "cProcessTKQC", "Login Via thất bại", 1);
                                    return;
                                }
                            }
                            if (await exist.FacebookHandler.RemoveSpendCap(adAccount))
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Gỡ GHCT thành công", 0);
                            }
                            else
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Gỡ GHCT thất bại", 1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void mởChromePEToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        object lockObject = new object();
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            semaphore.Wait();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Đang mở chrome...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string adAccountId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    ChromeHandler chromeHandler = exist.ChromeHandler;
                    lock (lockObject)
                    {
                        if (chromeHandler == null)
                        {
                            ChromeService chromeService = new ChromeService();
                            (ChromeService.StatusOpenChrome, string) tuple = chromeService.OpenChrome();
                            var (statusOpen, _) = tuple;
                            _ = tuple.Item2;
                            if (statusOpen == ChromeService.StatusOpenChrome.Closed)
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Vui lòng tắt chrome profile trước khi mở!", 1);
                                return;
                            }
                            chromeHandler = new ChromeHandler(chromeService, exist.Account);
                            if (!chromeHandler.LoginFacebook())
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Đăng nhập thất bại!", 1);
                                return;
                            }
                        }
                        else if (!chromeHandler.IsLive())
                        {
                            ChromeService chromeService2 = new ChromeService();
                            (ChromeService.StatusOpenChrome, string) tuple = chromeService2.OpenChrome();
                            var (statusOpen2, _) = tuple;
                            _ = tuple.Item2;
                            if (statusOpen2 == ChromeService.StatusOpenChrome.Closed)
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Vui lòng tắt chrome profile trước khi mở!", 1);
                                return;
                            }
                            chromeHandler = new ChromeHandler(chromeService2, exist.Account);
                            if (!chromeHandler.LoginFacebook())
                            {
                                UpdateGridCellAsync(row, "cProcessTKQC", "Đăng nhập thất bại!", 1);
                                return;
                            }
                        }
                        exist.ChromeHandler = chromeHandler;
                        FacebookHandlerContainer.Instance.UpdateFacebookHandlerProfile(exist);
                    }
                    chromeHandler.OpenNewTab("https://adsmanager.facebook.com/adsmanager/manage/campaigns?act=" + adAccountId + "&nav_entry_point=comet_bookmark&nav_source=comet", switchToLastTab: false);
                    UpdateGridCellAsync(row, "cProcessTKQC", "Mở chrome xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessTKQC", "Mở chrome Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Mở chrome TKQC: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void tạoTKQCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        int countAdAccount = (int)nudCountNumAdAccount.Value;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string currency = string.Empty;
                        string timeZone = string.Empty;
                        string region = string.Empty;
                        string partner = string.Empty;
                        string baseName = string.Empty;
                        Invoke((Action)delegate
                        {
                            currency = cbbCurrencyBM.Text.Split('(')[1].Replace(")", "");
                            timeZone = cbbTimeZoneBM.SelectedValue?.ToString();
                            region = tbRegion.Text;
                            partner = tbPartner.Text;
                            baseName = tbNameBM.Text;
                        });
                        for (int i = 0; i < countAdAccount; i++)
                        {
                            string name = $"{baseName} {i + 1}";
                            var (statusCreateAdAccount, message) = await facebookHandler.CreateAdAccountInBMVer2(businessId, name, currency, timeZone, partner);
                            if (statusCreateAdAccount != FacebookHandler.StatusCreateBM.Success)
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC thất bại => " + message, 1);
                                return;
                            }
                            UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC thành công: " + message, 0);
                        }
                        UpdateGridCellAsync(row, "cProcessBM", "Tạo TKQC thành công", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkQTVBMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Check Limit BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string businessId = row.Cells[1].Value.ToString();
                        List<BusinessUserDto> businessUsers = await facebookHandler.LoadUser(businessId);
                        if (businessUsers != null)
                        {
                            string businessUserId = string.Join(Environment.NewLine, businessUsers.Select((BusinessUserDto b) => b.UserName.Split()[0]));
                            row.Cells[13].Value = businessUserId.TrimEnd('|');
                        }
                        UpdateGridCellAsync(row, "cProcessBM", "Xong", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Load User Lỗi", 0);
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Check Limit BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void kíchNútKhángToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Check Limit BM", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string businessId = row.Cells[1].Value.ToString();
                        if (await facebookHandler.KickNutKhangBM(businessId))
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Kích nút kháng thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Kích nút kháng thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Kích nút kháng lỗi", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Check Limit BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnGetCookieIG_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvVia.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản cần Login!!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        _ = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        _loginTasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            AccountDto accountDto = GetAccountDtoByRow(row);
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    row.Cells["cProcess"].Value = "Processing...";
                    FacebookHandler instagramService = new FacebookHandler(accountDto, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                    FacebookHandler.StatusCookie statusLogin = await instagramService.LoginInstagram(0);
                    if (!accountDto.Cookie.Contains("c_user") && statusLogin != FacebookHandler.StatusCookie.Live)
                    {
                        row.Cells["cProcess"].Value = "Login IG thất bại=> Login Uid|Pass";
                        statusLogin = await instagramService.LoginInstagramUidPass();
                        if (statusLogin != FacebookHandler.StatusCookie.Live)
                        {
                            UpdateGridCellAsync(row, "cProcess", $"Log in thất bại => {statusLogin}", 1);
                            return;
                        }
                    }
                    UpdateGridCellAsync(row, "cProcess", "Log in business", 0);
                    if (!(await instagramService.LoginBusinessFacebook()))
                    {
                        row.Cells["cProcess"].Value = "Login vào Business thất bại";
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcess", "Log in Success => Get token", 0);
                        var (dtsg, token) = await instagramService.GetTokenAsync();
                        if (!string.IsNullOrEmpty(token))
                        {
                            row.Cells["cToken"].Value = token;
                        }
                        string cookie = instagramService.GetCookie();
                        if (!string.IsNullOrEmpty(token))
                        {
                            row.Cells["cCookie"].Value = cookie;
                        }
                        accountDto.Cookie = cookie;
                        accountDto.DTSGToken = dtsg;
                        accountDto.Token = token;
                        UpdateGridCellAsync(row, "cProcess", "Get Token Done", 0);
                        FacebookHandlerDto facebookHandlerDto = new FacebookHandlerDto
                        {
                            FacebookHandler = instagramService,
                            Account = accountDto,
                            IsLoggedIn = true
                        };
                        FacebookHandlerContainer.Instance.AddFacebookHandlerProfile(facebookHandlerDto);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Login: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore?.Release();
                }
            }));
        }
    }

    private async void tạoWAToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int countWA = (int)nudCountNumAdAccount.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang tạo WA...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    (int, int) isSuccess = await exist.FacebookHandler.KichAppVer2(businessId, countWA);
                    if (isSuccess.Item1 > 0)
                    {
                        UpdateGridCellAsync(row, "cProcessBM", $"Tạo WA thành công: {isSuccess.Item1}-Tạo WA thất bại: {isSuccess.Item2}", 0);
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcessBM", $"Tạo WA thành công: {isSuccess.Item1}-Tạo WA thất bại: {isSuccess.Item2}", 1);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Tạo WA !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void btnAppel_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        string captchaKey = tbCaptchaKey.Text;
        if (string.IsNullOrEmpty(captchaKey))
        {
            MessageBox.Show("Vui lòng nhập key captcha để thực hiện chức năng này!");
            return;
        }
        string viotpApiKey = tbViotpApiKey.Text;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        if (string.IsNullOrEmpty(viotpApiKey))
        {
            MessageBox.Show("Vui lòng nhập key phone để thực hiện chức năng này!");
            return;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Processing...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    UpdateGridCellAsync(row, "cProcessBM", "Bước 1: Lấy Id kháng BM...", 0);
                    string appealId = await facebookHandler.GetAppealId(businessId);
                    if (string.IsNullOrEmpty(appealId))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Không tìm thấy nút kháng BM", 1);
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Bước 2: Get captcha...", 0);
                        var (statusGetCaptcha, captchaPersist) = await facebookHandler.GetCaptchaPersist(businessId, appealId);
                        if (!statusGetCaptcha)
                        {
                            string text = captchaPersist;
                            string text2 = text;
                            if (text2 == "Empty StateHash")
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Không thể thao tác bước tiếp theo", 1);
                                return;
                            }
                            if (text2 == "Can't found captchaPersist")
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Không lấy được captcha để giải mã", 1);
                                return;
                            }
                        }
                        if (!string.IsNullOrEmpty(captchaPersist))
                        {
                            int retryResolveCaptcha = 3;
                            bool statusResolve = false;
                            while (!statusResolve && retryResolveCaptcha-- > 0)
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Tiến hành giải captcha...", 0);
                                CaptchaDto captchaDto = new CaptchaDto
                                {
                                    CaptchaKey = captchaKey,
                                    WebsiteUrl = "https://www.fbsbx.com/captcha/recaptcha/iframe/",
                                    WebsiteKey = "6LeyIlkaAAAAAE-EjcALU28lwxWPusUvGL3e0avS"
                                };
                                CaptchaService captchaService = new CaptchaService(captchaDto);
                                var (statusCreateTask, taskId) = await captchaService.CreateRecaptchaV2Task();
                                if (statusCreateTask == CaptchaService.StatusCreateTask.Failure)
                                {
                                    UpdateGridCellAsync(row, "cProcessBM", "Lỗi tạo task: " + taskId, 1);
                                    return;
                                }
                                DataGridViewHelper.SetStatusDataGridViewWithWait(row, "cProcessBM", 5);
                                UpdateGridCellAsync(row, "cProcessBM", "Lấy kết quả captcha sau khi giải", 0);
                                var (statusGetResult, captchaResponse) = await captchaService.GetTaskResult(taskId);
                                if (statusGetResult == CaptchaService.StatusResultTask.Failure)
                                {
                                    UpdateGridCellAsync(row, "cProcessBM", "Lỗi giải captcha: " + captchaResponse, 1);
                                    return;
                                }
                                int retryTime = 5;
                                while (statusGetResult == CaptchaService.StatusResultTask.Processing && retryTime-- > 0)
                                {
                                    DataGridViewHelper.SetStatusDataGridViewWithWait(row, "cProcessBM", 10, "Chưa giải xong. Đợi {time} giây...");
                                    UpdateGridCellAsync(row, "cProcessBM", "Lấy kết quả captcha sau khi giải", 0);
                                    (statusGetResult, captchaResponse) = await captchaService.GetTaskResult(taskId);
                                    if (statusGetResult == CaptchaService.StatusResultTask.Failure)
                                    {
                                        UpdateGridCellAsync(row, "cProcessBM", "Lỗi giải captcha: " + captchaResponse, 1);
                                        return;
                                    }
                                }
                                _ = string.Empty;
                                string message;
                                (statusResolve, message) = await facebookHandler.ResolveCaptcha(businessId, appealId, captchaPersist, captchaResponse);
                                if (!statusResolve && message == "Captcha")
                                {
                                    UpdateGridCellAsync(row, "cProcessBM", "Giải captcha lại", 1);
                                }
                                else if (statusResolve)
                                {
                                    UpdateGridCellAsync(row, "cProcessBM", "Giải captcha thành công!", 0);
                                    break;
                                }
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessBM", "Bước 3: Kiểm tra upload phone...", 0);
                        if (await facebookHandler.ValidUploadPhoneStep(businessId, appealId) == "contact_point_ui_state_set_contact_point")
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Tiếp hành nhập phone", 0);
                            ViotpDto viotpDto = new ViotpDto
                            {
                                ApiKey = viotpApiKey,
                                ServiceId = "7"
                            };
                            ViotpService viotpService = new ViotpService(viotpDto);
                            ViotpDataPhone viotpDataPhone = await viotpService.CreateTask();
                            if (viotpDataPhone == null)
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Không thể lấy số điện thoại!", 1);
                                return;
                            }
                            string phoneNumber = viotpDataPhone.PhoneNumber;
                            if (string.IsNullOrEmpty(await facebookHandler.UploadPhoneNumber(businessId, appealId, phoneNumber)))
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Không thể thêm số điện thoại!", 1);
                                return;
                            }
                            DataGridViewHelper.SetStatusDataGridViewWithWait(row, "cProcessBM", 10, "Đợi {time} giây lấy code...");
                            UpdateGridCellAsync(row, "cProcessBM", "Đang lấy code...", 0);
                            SmsMessageDto smsMessageDto = await viotpService.GetCodeData(viotpDataPhone.RequestId);
                            if (smsMessageDto == null)
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Không thể lấy số mã!", 1);
                                return;
                            }
                            string code = smsMessageDto.Code;
                            if (string.IsNullOrEmpty(await facebookHandler.UploadVerifyCode(businessId, appealId, code)))
                            {
                                UpdateGridCellAsync(row, "cProcessBM", "Không thể thao tác bước kế!", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessBM", "Bước 4: Upload phôi kháng...", 0);
                        string imageFilePath = string.Empty;
                        var (statusUploadImage, imageHash) = await facebookHandler.UploadImage(imageFilePath, businessId);
                        if (statusUploadImage)
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Upload phôi kháng thất bại!", 1);
                        }
                        else
                        {
                            await facebookHandler.SubmitImage(businessId, appealId, imageHash);
                            UpdateGridCellAsync(row, "cProcessBM", "Hoàn thành tất cả bước", 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Kháng BM lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void xóaWAToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang Xóa WA...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    await exist.FacebookHandler.DeleteAllWA(businessId);
                    UpdateGridCellAsync(row, "cProcessBM", "Xóa WA hoàn tất", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Lỗi Xóa WA!", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void checkWAToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang Check WA...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    JArray countWA = await exist.FacebookHandler.CheckWA(businessId);
                    UpdateGridCellAsync(row, "cProcessBM", $"Có {countWA.Count} WA", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Lỗi Xóa WA!", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void cHECKTKQCDIEToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string message = await facebookHandler.CheckRetrictBM(adAccountId);
                        UpdateGridCellAsync(row, "cProcessTKQC", message, 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void loadTKẨnToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        dtgvTKQC.Rows.Clear();
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null || exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    FacebookHandler facebookHandler = exist.FacebookHandler;
                    new List<AdAccountDto>();
                    string nextUrl = string.Empty;
                    List<AdAccountDto> adAccountDtos;
                    do
                    {
                        (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 0, 1, nextUrl);
                        foreach (AdAccountDto adAccount in adAccountDtos)
                        {
                            if (adAccount.AdAccountStatus.Contains("Live"))
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount.AccountId, uid, adAccount.AccountName, adAccount.AccountSpent, adAccount.AdAccountStatus, adAccount.Currency, adAccount.AccountLimit, adAccount.AccountThreshold, adAccount.AccountBalance, adAccount.SpendCap, adAccount.PaymentMethod, adAccount.TimeZone, adAccount.TypeAdAccount, adAccount.CreatedTime, adAccount.Owner, adAccount.BusinessCountryCode, adAccount.UserCount, adAccount.PartnerCount, adAccount.CampaignCount);
                                });
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    adAccountDtos.Clear();
                    nextUrl = string.Empty;
                    do
                    {
                        (adAccountDtos, nextUrl) = await facebookHandler.LoadAdAccountV2(businessId, 1, 1, nextUrl);
                        foreach (AdAccountDto adAccount2 in adAccountDtos)
                        {
                            if (adAccount2.AdAccountStatus.Contains("Live"))
                            {
                                dtgvTKQC.Invoke((Action)delegate
                                {
                                    dtgvTKQC.Rows.Add(dtgvTKQC.RowCount + 1, adAccount2.AccountId, uid, adAccount2.AccountName, adAccount2.AccountSpent, adAccount2.AdAccountStatus, adAccount2.Currency, adAccount2.AccountLimit, adAccount2.AccountThreshold, adAccount2.AccountBalance, adAccount2.SpendCap, adAccount2.PaymentMethod, adAccount2.TimeZone, adAccount2.TypeAdAccount, adAccount2.CreatedTime, adAccount2.Owner, adAccount2.BusinessCountryCode, adAccount2.UserCount, adAccount2.PartnerCount, adAccount2.CampaignCount);
                                });
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextUrl));
                    await UpdateLabelText(lblTotalCountTKQC, $"{dtgvTKQC.RowCount}");
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Load TKQC Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Load TKQC: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void btnCreateWA_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int countWA = (int)nudCountCreateWA.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang tạo WA...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    (int, int) isSuccess = await exist.FacebookHandler.CreateWA2(businessId, countWA);
                    if (isSuccess.Item1 > 0)
                    {
                        UpdateGridCellAsync(row, "cProcessBM", $"Tạo WA thành công: {isSuccess.Item1}-Tạo WA thất bại: {isSuccess.Item2}", 0);
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcessBM", $"Tạo WA thành công: {isSuccess.Item1}-Tạo WA thất bại: {isSuccess.Item2}", 1);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Tạo WA Lỗi!", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void checkLimitTKẨnToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Check Limit TK ẩn...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string businessId = row.Cells[1].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        List<AdAccountDto> hideAdAccounts = await facebookHandler.LoadTKQCHide(businessId);
                        if (hideAdAccounts == null || !hideAdAccounts.Any())
                        {
                            UpdateGridCellAsync(row, "cProcessBM", "Load thất bại hoặc không có tk ẩn!", 1);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessBM", $"Có {hideAdAccounts.Count} tk ẩn", 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error Process Out BM: " + ex3.StackTrace + "\n";
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void nhétTKQCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        string businessId = tbBMID.Text;
        if (string.IsNullOrEmpty(businessId))
        {
            MessageBox.Show("Vui lòng nhập BMID!");
            return;
        }
        new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Processing...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                        return;
                    }
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist?.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    string adAccount = row.Cells[1].Value?.ToString();
                    FacebookHandler facebookHandler = new FacebookHandler(exist.Account, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36", "");
                    facebookHandler.LoginCookie();
                    if (await facebookHandler.AddAdAccountIntoBM(businessId, adAccount))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Nhét thành công", 0);
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Nhét thất bại", 1);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    row.Cells["cProcessTKQC"].Value = ex2.Message;
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(1.75));
                }
            }));
            await Task.Delay(TimeSpan.FromSeconds(1.0));
        }
    }

    private async void mởBMChromeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow row in dtgvBM.Rows
                                              where row.Selected || row.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select row).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        object lockObject = new object();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        var rows = from DataGridViewRow row in selectedRows
                   where !row.IsNewRow
                   select new
                   {
                       ID = row.Cells[1].Value?.ToString(),
                       UID = row.Cells[2].Value?.ToString()
                   } into item
                   where item.UID != null
                   select item;
        var groupedData = from item in rows
                          group item by item.UID into g
                          select new
                          {
                              GroupKey = g.Key,
                              Values = g.Select(item => item.ID).ToList()
                          };
        foreach (var group in groupedData)
        {
            semaphore.Wait();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = group.GroupKey;
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    ChromeHandler chromeHandler = exist.ChromeHandler;
                    lock (lockObject)
                    {
                        if (chromeHandler == null)
                        {
                            ChromeService chromeService = new ChromeService
                            {
                                Proxy = proxy
                            };
                            (ChromeService.StatusOpenChrome, string) tuple = chromeService.OpenChrome();
                            var (statusOpen, _) = tuple;
                            _ = tuple.Item2;
                            if (statusOpen == ChromeService.StatusOpenChrome.Closed)
                            {
                                List<int> lstIndex = FindListRowIndex(dtgvBM, uid);
                                foreach (int index in lstIndex)
                                {
                                    DataGridViewRow row = dtgvBM.Rows[index];
                                    UpdateGridCellAsync(row, "cProcessBM", "Vui lòng tắt chrome profile trước khi mở!", 1);
                                }
                                return;
                            }
                            chromeHandler = new ChromeHandler(chromeService, exist.Account);
                            if (!chromeHandler.LoginFacebook())
                            {
                                List<int> lstIndex2 = FindListRowIndex(dtgvBM, uid);
                                foreach (int index2 in lstIndex2)
                                {
                                    DataGridViewRow row2 = dtgvBM.Rows[index2];
                                    UpdateGridCellAsync(row2, "cProcessBM", "Đăng nhập thất bại!", 1);
                                }
                                return;
                            }
                        }
                        else if (!chromeHandler.IsLive())
                        {
                            ChromeService chromeService2 = new ChromeService();
                            (ChromeService.StatusOpenChrome, string) tuple = chromeService2.OpenChrome();
                            var (statusOpen2, _) = tuple;
                            _ = tuple.Item2;
                            if (statusOpen2 == ChromeService.StatusOpenChrome.Closed)
                            {
                                List<int> lstIndex3 = FindListRowIndex(dtgvBM, uid);
                                foreach (int index3 in lstIndex3)
                                {
                                    DataGridViewRow row3 = dtgvBM.Rows[index3];
                                    UpdateGridCellAsync(row3, "cProcessBM", "Vui lòng tắt chrome profile trước khi mở!", 1);
                                }
                                return;
                            }
                            chromeHandler = new ChromeHandler(chromeService2, exist.Account);
                            if (!chromeHandler.LoginFacebook())
                            {
                                List<int> lstIndex4 = FindListRowIndex(dtgvBM, uid);
                                foreach (int index4 in lstIndex4)
                                {
                                    DataGridViewRow row4 = dtgvBM.Rows[index4];
                                    UpdateGridCellAsync(row4, "cProcessBM", "Đăng nhập thất bại!", 1);
                                }
                                return;
                            }
                        }
                        exist.ChromeHandler = chromeHandler;
                        FacebookHandlerContainer.Instance.UpdateFacebookHandlerProfile(exist);
                    }
                    foreach (string businessId in group.Values)
                    {
                        int index5 = FindRowIndexByUid(dtgvBM, businessId);
                        if (index5 == -1)
                        {
                            return;
                        }
                        DataGridViewRow row5 = dtgvBM.Rows[index5];
                        UpdateGridCellAsync(row5, "cProcessBM", "Đang mở chrome...", 0);
                        chromeHandler.OpenNewTab("https://business.facebook.com/business-support-home/" + businessId + "/?source=mbs_more_tools_flyout", switchToLastTab: true);
                        await Task.Delay(TimeSpan.FromSeconds(2.0));
                        chromeHandler.RemoveBlock();
                        UpdateGridCellAsync(row5, "cProcessBM", "Mở chrome xong", 0);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Mở chrome: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void xóaQuảnTrịBằngIGToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvUser.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        _ = cbbTypeMail.SelectedIndex;
        _ = cbbTypeRole.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cStatusUser", "Đang xóa....", 0);
                    string uid = row.Cells[1].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cStatusUser", "Nhập kèm UID", 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string userId = row.Cells[2].Value.ToString();
                        string businessId = row.Cells[3].Value.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.DeleteUserByIG(userId, businessId))
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Delete thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cStatusUser", "Delete thất bại", 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    rtbLog.Invoke((Action)delegate
                    {
                        RichTextBox richTextBox = rtbLog;
                        richTextBox.Text = richTextBox.Text + "Error DeleteUser: " + ex3.StackTrace + "\n";
                    });
                    UpdateGridCellAsync(row, "cStatusUser", ex3.Message ?? "", 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void kíchAppToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang kích App...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    await exist.FacebookHandler.KichApp3(businessId);
                    UpdateGridCellAsync(row, "cProcessBM", "Kích App xong", 0);
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Kích App Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private async void btnCheckMail_Click(object sender, EventArgs e)
    {
        try
        {
            dtgvHotmail.Rows.Clear();
            if (cbbDomain.SelectedIndex != 0)
            {
                return;
            }
            string emailRaw = tbUsername.Text;
            string[] splitEmail = emailRaw.Split('|');
            HotmailDto hotmailDto = new HotmailDto
            {
                Username = splitEmail[0],
                Password = splitEmail[1],
                RefreshToken = splitEmail[2],
                ClientId = splitEmail[3]
            };
            HotmailHelper hotmailHelper = new HotmailHelper(hotmailDto);
            List<MailMessageDto> responseMailMessages = ((!ckbGetLinkBM.Checked) ? (await hotmailHelper.ReadMailMessageOAuth2Ver2()) : (await hotmailHelper.GetLinkInvite2()));
            if (responseMailMessages == null)
            {
                MessageBox.Show("Get Access Token thất bại!");
                return;
            }
            if (!responseMailMessages.Any())
            {
                MessageBox.Show("Đọc mail thất bại hoặc không có thư!");
                return;
            }
            foreach (MailMessageDto mailMessage in responseMailMessages)
            {
                dtgvHotmail.Invoke((Action)delegate
                {
                    dtgvHotmail.Rows.Add(dtgvHotmail.RowCount + 1, mailMessage.Sender, mailMessage.ReceiveTime, mailMessage.Subject, mailMessage.Code, mailMessage.Message, mailMessage.BusinessLink);
                });
            }
        }
        catch
        {
            MessageBox.Show("Đọc mail lỗi!");
        }
    }

    private void dtgvHotmail_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            DataGridViewRow row = dtgvHotmail.Rows[e.RowIndex];
            string valueCol3 = row.Cells["cBody"].Value?.ToString();
            webBrowser1.DocumentText = valueCol3;
        }
    }

    private async void btnDeleteAll_Click(object sender, EventArgs e)
    {
        string emailRaw = tbUsername.Text;
        string[] splitEmail = emailRaw.Split('|');
        HotmailDto hotmailDto = new HotmailDto
        {
            Username = splitEmail[0],
            Password = splitEmail[1],
            RefreshToken = splitEmail[2],
            ClientId = splitEmail[3]
        };
        HotmailHelper hotmailHelper = new HotmailHelper(hotmailDto);
        switch (await hotmailHelper.DeleteAllMessagesAsync())
        {
            case -1:
                MessageBox.Show("Get token mail thất bại!");
                break;
            case 0:
                MessageBox.Show("Xóa mail thất bại!");
                break;
            default:
                MessageBox.Show("Xóa mail thành công");
                break;
        }
    }

    private async void xóaQTVIGToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Processing....", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccount = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        UpdateGridCellAsync(row, "cProcessTKQC", "Đang xóa QTV và đối tác", 0);
                        var (statusOutTKQC, successCount, failedCount) = await facebookHandler.RemoveAllQTVByIG(adAccount);
                        if (statusOutTKQC)
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", $"Xóa thành công: {successCount} - Thất bại: {failedCount}", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", $"Xóa thành công: {successCount} - Thất bại: {failedCount}", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    row.Cells["cProcessTKQC"].Value = ex.Message;
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void kíchAppIGToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvBM.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _tasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            await semaphore.WaitAsync();
            _tasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessBM", "Đang kích App...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    string businessId = row.Cells[1].Value?.ToString();
                    int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                    DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                    AccountDto accountDto = GetAccountDtoByRow(rowVia);
                    FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                    if (exist == null || exist.FacebookHandler == null)
                    {
                        await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                        exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                        if (exist.FacebookHandler == null)
                        {
                            UpdateGridCellAsync(row, "cProcess", "Login Via thất bại", 1);
                            return;
                        }
                    }
                    UpdateGridCellAsync(row, "cProcessBM", "Đang xóa all WA...", 0);
                    await exist.FacebookHandler.DeleteAllWA(businessId);
                    UpdateGridCellAsync(row, "cProcessBM", "Tiến hành kích App", 0);
                    if ((await exist.FacebookHandler.KichAppVer2(businessId, 5)).Item1 > 0)
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Kích App xong", 0);
                    }
                    else
                    {
                        UpdateGridCellAsync(row, "cProcessBM", "Kích thất bại", 1);
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    Exception ex3 = ex2;
                    UpdateGridCellAsync(row, "cProcessBM", "Kích App Lỗi !", 1);
                    rtbLog.Invoke((Action)delegate
                    {
                        rtbLog.AppendText("Error Process Kích App: " + ex3.StackTrace + "\n");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(_tasks);
    }

    private void dánIGToolStripMenuItem_Click(object sender, EventArgs e)
    {
        dtgvIG.Rows.Clear();
        string clipboard = Clipboard.GetText();
        if (string.IsNullOrEmpty(clipboard))
        {
            MessageBox.Show("Vui lòng coppy IG trước khi kích");
            return;
        }
        string[] lines = (from x in clipboard.Split('\n')
                          where !string.IsNullOrEmpty(x)
                          select x).ToArray();
        string[] array = lines;
        foreach (string item in array)
        {
            string[] rawData = item.Split('|');
            string username = rawData[0];
            string password = rawData[1];
            string key2FA = rawData.Where((string x) => x.Length == 32).FirstOrDefault();
            string cookie = rawData.Where((string x) => x.Contains("ds_user")).FirstOrDefault();
            dtgvIG.Rows.Add(dtgvIG.RowCount + 1, username, password, key2FA, cookie);
        }
    }

    private async void checkIGToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvIG.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> tasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            tasks.Add(Task.Run(async delegate
            {
                try
                {
                    InstagramAccountDto instagramDto = new InstagramAccountDto();
                    string username = row.Cells[1].Value.ToString();
                    string password = row.Cells[2].Value.ToString();
                    string key2FA = row.Cells[3].Value?.ToString();
                    string cookie = row.Cells[4].Value?.ToString();
                    instagramDto.Username = username;
                    instagramDto.Password = password;
                    instagramDto.Key2FA = key2FA;
                    instagramDto.Cookie = cookie;
                    InstagramService instagramService = new InstagramService(instagramDto);
                    switch (await instagramService.LoginInstagram())
                    {
                        case InstagramService.StatusLoginIG.Success:
                            row.Cells["cProcessIG"].Value = "Login thành công";
                            break;
                        case InstagramService.StatusLoginIG.Failed:
                            row.Cells["cProcessIG"].Value = "Login thất bại";
                            break;
                        default:
                            row.Cells["cProcessIG"].Value = "Checkpoint";
                            break;
                    }
                }
                catch
                {
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(tasks);
    }

    private async void checkKíchToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvIG.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> tasks = new List<Task>();
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            tasks.Add(Task.Run(async delegate
            {
                try
                {
                    row.Cells["cProcessIG"].Value = "Login IG...";
                    InstagramAccountDto instagramDto = new InstagramAccountDto();
                    string username = row.Cells[1].Value.ToString();
                    string password = row.Cells[2].Value.ToString();
                    string key2FA = row.Cells[3].Value?.ToString();
                    string cookie = row.Cells[4].Value?.ToString();
                    instagramDto.Username = username;
                    instagramDto.Password = password;
                    instagramDto.Key2FA = key2FA;
                    instagramDto.Cookie = cookie;
                    InstagramService instagramService = new InstagramService(instagramDto);
                    switch (await instagramService.LoginInstagram())
                    {
                        case InstagramService.StatusLoginIG.Success:
                            row.Cells["cProcessIG"].Value = "Login thành công => Kiểm tra IG kích";
                            await instagramService.ConvertAccount();
                            if (await instagramService.CheckKich())
                            {
                                row.Cells["cProcessIG"].Value = "IG kích được";
                            }
                            else
                            {
                                row.Cells["cProcessIG"].Value = "IG không kích được";
                            }
                            break;
                        case InstagramService.StatusLoginIG.Failed:
                            row.Cells["cProcessIG"].Value = "Login thất bại";
                            break;
                        default:
                            row.Cells["cProcessIG"].Value = "Checkpoint";
                            break;
                    }
                }
                catch
                {
                    row.Cells["cProcessIG"].Value = "Lỗi kiểm tra IG kích";
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(tasks);
    }

    private async void thêmVàoDòng2BMToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành thêm TKQC vào BM", 0);
                        string adAccount = row.Cells[1].Value?.ToString();
                        string businessId = row.Cells["cBussinessId"].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.AddAdAccountIntoBM2(adAccount, businessId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Thêm thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Thêm thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnChangeInforAdAccount_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string currency = string.Empty;
        string timeZone = string.Empty;
        string region = string.Empty;
        Invoke((Action)delegate
        {
            currency = cbbCurrencyAdAccount.Text.Split('(')[1].Replace(")", "");
            string obj = cbbTimeZoneAdAccount.Text;
            timeZone = ((obj != null) ? obj.ToString().Split(':')[0] : null);
            region = tbRegionAdAccount.Text;
        });
        AdAccountInformationDto adAccountInfor = new AdAccountInformationDto
        {
            Currency = currency,
            TimeZone = timeZone,
            CountryCode = region
        };
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành đổi thông tin TKQC", 0);
                        string adAccountId = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.ChangeInfoTKQC(adAccountInfor, adAccountId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Đổi thông tin thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Đổi thông tin thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void btnLoadBusinessUserName_Click(object sender, EventArgs e)
    {
        string businessId = tbBusinessId.Text;
        DataGridViewRow rowVia = dtgvVia.Rows[0];
        string uid = rowVia.Cells[1].Value?.ToString();
        if (string.IsNullOrEmpty(uid))
        {
            MessageBox.Show("Chưa nhập Via để thao tác");
            return;
        }
        AccountDto accountDto = GetAccountDtoByRow(rowVia);
        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
        if (exist == null || exist.FacebookHandler == null)
        {
            await ProcessLoginFacebook(accountDto, rowVia, string.Empty, 0);
            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
            if (exist?.FacebookHandler == null)
            {
                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                return;
            }
        }
        FacebookHandler facebookHandler = exist.FacebookHandler;
        List<BusinessUserDto> businessUserDtos = await facebookHandler.LoadUser(businessId);
        if (businessUserDtos == null)
        {
            MessageBox.Show("Load User thất bại!");
            return;
        }
        cbbBusinessUserName.ValueMember = "UserId";
        cbbBusinessUserName.DisplayMember = "UserName";
        cbbBusinessUserName.DataSource = businessUserDtos;
    }

    private async void btnAddPermission_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        if (cbbBusinessUserName.SelectedIndex == -1)
        {
            return;
        }
        string selectedUserId = cbbBusinessUserName.SelectedValue?.ToString();
        if (string.IsNullOrEmpty(selectedUserId))
        {
            MessageBox.Show("Vui lòng chọn Load trước để thực hiện chức năng này!");
            return;
        }
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành thêm quyền TKQC", 0);
                        string adAccountId = row.Cells[1].Value?.ToString();
                        string businessId = tbBusinessId.Text;
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.AssignPermissionUserIntoAdAccount(adAccountId, businessId, selectedUserId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Thêm thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Thêm thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void checkPTTTToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành Check PTTT TKQC", 0);
                        string adAccountId = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        string statusAddAccountIntoBM = await facebookHandler.CheckPTTT(adAccountId);
                        UpdateGridCellAsync(row, "cProcessTKQC", statusAddAccountIntoBM ?? "", 0);
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }


    private async void deleteCreditCardToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Processing...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        if (string.IsNullOrEmpty(adAccountId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "ID tài khoản quảng cáo trống", 1);
                        }
                        else
                        {
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            bool isRemoved = await facebookHandler.RemoveCreditCard(adAccountId, null);
                            UpdateGridCellAsync(row, "cProcessTKQC", isRemoved ? "Xóa thẻ thành công" : "Xóa thẻ thất bại", isRemoved ? 0 : 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void deleteCampToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", "Processing...", 0);
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        string adAccountId = row.Cells[1].Value?.ToString();
                        if (string.IsNullOrEmpty(adAccountId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "ID tài khoản quảng cáo trống", 1);
                        }
                        else
                        {
                            FacebookHandler facebookHandler = exist.FacebookHandler;
                            bool archived = await facebookHandler.ArchiveAllCampaigns(adAccountId);
                            UpdateGridCellAsync(row, "cProcessTKQC", archived ? "Xóa camp thành công" : "Xóa camp thất bại", archived ? 0 : 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    private async void createRuleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<DataGridViewRow> selectedRows = (from DataGridViewRow dataGridViewRow in dtgvTKQC.Rows
                                              where dataGridViewRow.Selected || dataGridViewRow.Cells.Cast<DataGridViewCell>().Any((DataGridViewCell cell) => cell.Selected)
                                              select dataGridViewRow).ToList();
        if (selectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn tài khoản để thực hiện chức năng này!");
            return;
        }
        int maxThread = (int)nudCountThreads.Value;
        _ = cbbTypeLogin.SelectedIndex;
        int typeProxy = cbbTypeProxy.SelectedIndex;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThread, maxThread);
        List<Task> _loginTasks = new List<Task>();
        string proxy = string.Empty;
        if (typeProxy == 1)
        {
            proxy = tbProxy.Text;
        }
        foreach (DataGridViewRow row in selectedRows)
        {
            await semaphore.WaitAsync();
            _loginTasks.Add(Task.Run(async delegate
            {
                try
                {
                    string uid = row.Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(uid))
                    {
                        UpdateGridCellAsync(row, "cProcessTKQC", "Không tìm thấy account với Uid: " + uid, 1);
                    }
                    else
                    {
                        int rowViaIndex = FindRowIndexByUid(dtgvVia, uid);
                        DataGridViewRow rowVia = dtgvVia.Rows[rowViaIndex];
                        AccountDto accountDto = GetAccountDtoByRow(rowVia);
                        FacebookHandlerDto exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == uid);
                        if (exist == null || exist.FacebookHandler == null)
                        {
                            await ProcessLoginFacebook(accountDto, rowVia, proxy, 0);
                            exist = FacebookHandlerContainer.Instance.FacebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid == accountDto.Uid);
                            if (exist?.FacebookHandler == null)
                            {
                                UpdateGridCellAsync(rowVia, "cProcess", "Login Via thất bại", 1);
                                return;
                            }
                        }
                        UpdateGridCellAsync(row, "cProcessTKQC", "Tiến hành tạo quy tắc", 0);
                        string adAccountId = row.Cells[1].Value?.ToString();
                        FacebookHandler facebookHandler = exist.FacebookHandler;
                        if (await facebookHandler.CreateRule(adAccountId))
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Tạo quy tắc thành công", 0);
                        }
                        else
                        {
                            UpdateGridCellAsync(row, "cProcessTKQC", "Tạo quy tắc thất bại", 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateGridCellAsync(row, "cProcessTKQC", ex.Message, 1);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TQI.NBTeam.frmMain));
        this.panel1 = new System.Windows.Forms.Panel();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.tabControl2 = new System.Windows.Forms.TabControl();
        this.tabPage4 = new System.Windows.Forms.TabPage();
        this.btnGetCookieIG = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.btnLoadTKQC = new System.Windows.Forms.Button();
        this.cbbTypeLoadTKQC = new System.Windows.Forms.ComboBox();
        this.btnLoadBM = new System.Windows.Forms.Button();
        this.cbbTypeLoadBM = new System.Windows.Forms.ComboBox();
        this.nudCountThreads = new System.Windows.Forms.NumericUpDown();
        this.label2 = new System.Windows.Forms.Label();
        this.btnStop = new System.Windows.Forms.Button();
        this.btnStart = new System.Windows.Forms.Button();
        this.dtgvVia = new System.Windows.Forms.DataGridView();
        this.cStt = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUid = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cKey2FA = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCookie = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cToken = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cProcess = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ctmnsVia = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.dánViaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaViaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tabPage5 = new System.Windows.Forms.TabPage();
        this.label27 = new System.Windows.Forms.Label();
        this.tbProxy = new System.Windows.Forms.TextBox();
        this.label5 = new System.Windows.Forms.Label();
        this.rtbLog = new System.Windows.Forms.RichTextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.cbbTypeProxy = new System.Windows.Forms.ComboBox();
        this.label1 = new System.Windows.Forms.Label();
        this.cbbTypeLogin = new System.Windows.Forms.ComboBox();
        this.tsVia = new System.Windows.Forms.ToolStrip();
        this.lblStatus = new System.Windows.Forms.ToolStripLabel();
        this.lblTextTotalCount = new System.Windows.Forms.ToolStripLabel();
        this.lblTotalCount = new System.Windows.Forms.ToolStripLabel();
        this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
        this.lblCountSelected = new System.Windows.Forms.ToolStripLabel();
        this.tabPage2 = new System.Windows.Forms.TabPage();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.tabControl3 = new System.Windows.Forms.TabControl();
        this.tabPage7 = new System.Windows.Forms.TabPage();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.label21 = new System.Windows.Forms.Label();
        this.tbMailPass = new System.Windows.Forms.TextBox();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.btnOutBM = new System.Windows.Forms.Button();
        this.label7 = new System.Windows.Forms.Label();
        this.cbbTypeMail = new System.Windows.Forms.ComboBox();
        this.cbbTypeRole = new System.Windows.Forms.ComboBox();
        this.btnBackUp = new System.Windows.Forms.Button();
        this.label6 = new System.Windows.Forms.Label();
        this.lblCountLinkBM = new System.Windows.Forms.Label();
        this.rtbLinkBM = new System.Windows.Forms.RichTextBox();
        this.label10 = new System.Windows.Forms.Label();
        this.tabPage8 = new System.Windows.Forms.TabPage();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.label19 = new System.Windows.Forms.Label();
        this.nudCountNumAdAccount = new System.Windows.Forms.NumericUpDown();
        this.tbNameBM = new System.Windows.Forms.TextBox();
        this.label14 = new System.Windows.Forms.Label();
        this.tbRegion = new System.Windows.Forms.TextBox();
        this.label8 = new System.Windows.Forms.Label();
        this.label13 = new System.Windows.Forms.Label();
        this.cbbCurrencyBM = new System.Windows.Forms.ComboBox();
        this.btnCreateAdAccount = new System.Windows.Forms.Button();
        this.cbbTimeZoneBM = new System.Windows.Forms.ComboBox();
        this.label12 = new System.Windows.Forms.Label();
        this.label9 = new System.Windows.Forms.Label();
        this.label11 = new System.Windows.Forms.Label();
        this.tbPartner = new System.Windows.Forms.TextBox();
        this.cbbTypeCreateAdAccount = new System.Windows.Forms.ComboBox();
        this.tabPage11 = new System.Windows.Forms.TabPage();
        this.btnChangeEmail = new System.Windows.Forms.Button();
        this.label20 = new System.Windows.Forms.Label();
        this.tbMailBM = new System.Windows.Forms.TextBox();
        this.btnChangeNameBM = new System.Windows.Forms.Button();
        this.label17 = new System.Windows.Forms.Label();
        this.tbBusinessName = new System.Windows.Forms.TextBox();
        this.tabPage10 = new System.Windows.Forms.TabPage();
        this.btnJoinBM = new System.Windows.Forms.Button();
        this.tbLinkBM = new System.Windows.Forms.TextBox();
        this.tabPage12 = new System.Windows.Forms.TabPage();
        this.label23 = new System.Windows.Forms.Label();
        this.tbViotpApiKey = new System.Windows.Forms.TextBox();
        this.btnAppeal = new System.Windows.Forms.Button();
        this.label22 = new System.Windows.Forms.Label();
        this.tbCaptchaKey = new System.Windows.Forms.TextBox();
        this.tabPage14 = new System.Windows.Forms.TabPage();
        this.btnCreateWA = new System.Windows.Forms.Button();
        this.label24 = new System.Windows.Forms.Label();
        this.nudCountCreateWA = new System.Windows.Forms.NumericUpDown();
        this.tsBM = new System.Windows.Forms.ToolStrip();
        this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
        this.lblCountTotalBM = new System.Windows.Forms.ToolStripLabel();
        this.dtgvBM = new System.Windows.Forms.DataGridView();
        this.cSttBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cIDBMVia = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUidVia = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cNameBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBMType = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cVerifyBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCreateAdAccountBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCountQTVBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cInfoBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCreateTimeBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cStatusBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cLimitAdAccountBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cAdAccountCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cQTVBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cProcessBM = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ctmnsBM = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.dánIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadTKQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadLiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadDieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadTKẨnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkInfoBMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkLimitBMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkBMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkQTVBMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkWAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.loadAssetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.mởBMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.kíchAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.kíchAppToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.kíchAppIGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.kíchNútKhángToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tạoWAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tạoTKQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaWAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkLimitTKẨnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.mởBMChromeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.groupBox5 = new System.Windows.Forms.GroupBox();
        this.tabControl4 = new System.Windows.Forms.TabControl();
        this.tabPage9 = new System.Windows.Forms.TabPage();
        this.groupBox9 = new System.Windows.Forms.GroupBox();
        this.btnShareTKQC = new System.Windows.Forms.Button();
        this.tbLstUidVia = new System.Windows.Forms.TextBox();
        this.groupBox7 = new System.Windows.Forms.GroupBox();
        this.btnBuildBM = new System.Windows.Forms.Button();
        this.tbBMID = new System.Windows.Forms.TextBox();
        this.label18 = new System.Windows.Forms.Label();
        this.groupBox6 = new System.Windows.Forms.GroupBox();
        this.btnRemovePartner = new System.Windows.Forms.Button();
        this.btnAssignPartner = new System.Windows.Forms.Button();
        this.tbPartnerId = new System.Windows.Forms.TextBox();
        this.label16 = new System.Windows.Forms.Label();
        this.label15 = new System.Windows.Forms.Label();
        this.cbbPermitTask = new System.Windows.Forms.ComboBox();
        this.tabPage13 = new System.Windows.Forms.TabPage();
        this.groupBox8 = new System.Windows.Forms.GroupBox();
        this.btnRemoveGHCT = new System.Windows.Forms.Button();
        this.btnSetGHCT = new System.Windows.Forms.Button();
        this.tbGHCT = new System.Windows.Forms.TextBox();
        this.tabPage19 = new System.Windows.Forms.TabPage();
        this.btnChangeInforAdAccount = new System.Windows.Forms.Button();
        this.tbNameAdAccount = new System.Windows.Forms.TextBox();
        this.label28 = new System.Windows.Forms.Label();
        this.tbRegionAdAccount = new System.Windows.Forms.TextBox();
        this.label29 = new System.Windows.Forms.Label();
        this.cbbCurrencyAdAccount = new System.Windows.Forms.ComboBox();
        this.cbbTimeZoneAdAccount = new System.Windows.Forms.ComboBox();
        this.label30 = new System.Windows.Forms.Label();
        this.label31 = new System.Windows.Forms.Label();
        this.tabPage20 = new System.Windows.Forms.TabPage();
        this.btnAddPermission = new System.Windows.Forms.Button();
        this.btnLoadBusinessUserName = new System.Windows.Forms.Button();
        this.cbbBusinessUserName = new System.Windows.Forms.ComboBox();
        this.label32 = new System.Windows.Forms.Label();
        this.tbBusinessId = new System.Windows.Forms.TextBox();
        this.dtgvTKQC = new System.Windows.Forms.DataGridView();
        this.cSttTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cIDTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUidViaTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cNameTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cAccountSpent = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cStatusTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCurrencyTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cLimitTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBalanceTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cSpendCap = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cPaymentTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cTimeZoneTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBusiness = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCreateTimeTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cOwnerTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cRegionTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUserCountTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cPartnerCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCampaignTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBussinessId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cProcessTKQC = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ctmnsTKQC = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.tsmPasteBusinessId = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        this.checkPTTTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.createRuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.deleteCreditCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.deleteCampToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.thoátTKQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.thêmVàoDòng2BMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaQTVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaQTVIGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.thoátTKQCToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.đóngTKQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.mởTKQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.gánQuyềnAddThẻToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.mởChromePEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkCampaignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkBillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.payToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.cHECKTKQCDIEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.nhétTKQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.tsTKQC = new System.Windows.Forms.ToolStrip();
        this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
        this.lblTotalCountTKQC = new System.Windows.Forms.ToolStripLabel();
        this.tabPage6 = new System.Windows.Forms.TabPage();
        this.dtgvUser = new System.Windows.Forms.DataGridView();
        this.cSttUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUidUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBMIdUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cMailUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cNameUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cRoleUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cStatusUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ctmnsUser = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.xóaQuảnTrịToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaQuảnTrịTrước7NgàyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaQuảnTrịBằngIGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaLờiMờiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.hạQuyềnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.xóaLờiMờiToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.tabPage15 = new System.Windows.Forms.TabPage();
        this.tabControl5 = new System.Windows.Forms.TabControl();
        this.tabPage16 = new System.Windows.Forms.TabPage();
        this.webBrowser1 = new System.Windows.Forms.WebBrowser();
        this.tabPage17 = new System.Windows.Forms.TabPage();
        this.dtgvHotmail = new System.Windows.Forms.DataGridView();
        this.cSttMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cSender = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cReceiveTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cBusinessLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ctmnsHotmail = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.copyLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.groupBox10 = new System.Windows.Forms.GroupBox();
        this.ckbGetLinkBM = new System.Windows.Forms.CheckBox();
        this.btnDeleteAll = new System.Windows.Forms.Button();
        this.btnCheckMail = new System.Windows.Forms.Button();
        this.cbbDomain = new System.Windows.Forms.ComboBox();
        this.label26 = new System.Windows.Forms.Label();
        this.label25 = new System.Windows.Forms.Label();
        this.tbUsername = new System.Windows.Forms.TextBox();
        this.tabPage18 = new System.Windows.Forms.TabPage();
        this.dtgvIG = new System.Windows.Forms.DataGridView();
        this.cSttIG = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cUsernameIG = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cPasswordIG = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cKey2FAIG = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cCookieIG = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.cProcessIG = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ctmsIG = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.dánIGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkIGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.checkKíchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.panel1.SuspendLayout();
        this.tabControl1.SuspendLayout();
        this.tabPage1.SuspendLayout();
        this.tabControl2.SuspendLayout();
        this.tabPage4.SuspendLayout();
        this.groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.nudCountThreads).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.dtgvVia).BeginInit();
        this.ctmnsVia.SuspendLayout();
        this.tabPage5.SuspendLayout();
        this.tsVia.SuspendLayout();
        this.tabPage2.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.tabControl3.SuspendLayout();
        this.tabPage7.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.tabPage8.SuspendLayout();
        this.groupBox4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.nudCountNumAdAccount).BeginInit();
        this.tabPage11.SuspendLayout();
        this.tabPage10.SuspendLayout();
        this.tabPage12.SuspendLayout();
        this.tabPage14.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.nudCountCreateWA).BeginInit();
        this.tsBM.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvBM).BeginInit();
        this.ctmnsBM.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.groupBox5.SuspendLayout();
        this.tabControl4.SuspendLayout();
        this.tabPage9.SuspendLayout();
        this.groupBox9.SuspendLayout();
        this.groupBox7.SuspendLayout();
        this.groupBox6.SuspendLayout();
        this.tabPage13.SuspendLayout();
        this.groupBox8.SuspendLayout();
        this.tabPage19.SuspendLayout();
        this.tabPage20.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvTKQC).BeginInit();
        this.ctmnsTKQC.SuspendLayout();
        this.tsTKQC.SuspendLayout();
        this.tabPage6.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvUser).BeginInit();
        this.ctmnsUser.SuspendLayout();
        this.tabPage15.SuspendLayout();
        this.tabControl5.SuspendLayout();
        this.tabPage16.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvHotmail).BeginInit();
        this.ctmnsHotmail.SuspendLayout();
        this.groupBox10.SuspendLayout();
        this.tabPage18.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvIG).BeginInit();
        this.ctmsIG.SuspendLayout();
        base.SuspendLayout();
        this.panel1.Controls.Add(this.tabControl1);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.panel1.Location = new System.Drawing.Point(0, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(1519, 565);
        this.panel1.TabIndex = 0;
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Controls.Add(this.tabPage2);
        this.tabControl1.Controls.Add(this.tabPage3);
        this.tabControl1.Controls.Add(this.tabPage6);
        this.tabControl1.Controls.Add(this.tabPage15);
        this.tabControl1.Controls.Add(this.tabPage18);
        this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl1.Location = new System.Drawing.Point(0, 0);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(1519, 565);
        this.tabControl1.TabIndex = 0;
        this.tabPage1.Controls.Add(this.tabControl2);
        this.tabPage1.Controls.Add(this.tsVia);
        this.tabPage1.Location = new System.Drawing.Point(4, 25);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(1511, 536);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "Cấu Hình Via";
        this.tabPage1.UseVisualStyleBackColor = true;
        this.tabControl2.Controls.Add(this.tabPage4);
        this.tabControl2.Controls.Add(this.tabPage5);
        this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl2.Location = new System.Drawing.Point(3, 3);
        this.tabControl2.Name = "tabControl2";
        this.tabControl2.SelectedIndex = 0;
        this.tabControl2.Size = new System.Drawing.Size(1505, 505);
        this.tabControl2.TabIndex = 2;
        this.tabPage4.Controls.Add(this.btnGetCookieIG);
        this.tabPage4.Controls.Add(this.groupBox1);
        this.tabPage4.Controls.Add(this.nudCountThreads);
        this.tabPage4.Controls.Add(this.label2);
        this.tabPage4.Controls.Add(this.btnStop);
        this.tabPage4.Controls.Add(this.btnStart);
        this.tabPage4.Controls.Add(this.dtgvVia);
        this.tabPage4.Location = new System.Drawing.Point(4, 25);
        this.tabPage4.Name = "tabPage4";
        this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage4.Size = new System.Drawing.Size(1497, 476);
        this.tabPage4.TabIndex = 0;
        this.tabPage4.Text = "Hiển thị";
        this.tabPage4.UseVisualStyleBackColor = true;
        this.btnGetCookieIG.Location = new System.Drawing.Point(372, 5);
        this.btnGetCookieIG.Name = "btnGetCookieIG";
        this.btnGetCookieIG.Size = new System.Drawing.Size(108, 28);
        this.btnGetCookieIG.TabIndex = 7;
        this.btnGetCookieIG.Text = "Get Cookie IG";
        this.btnGetCookieIG.UseVisualStyleBackColor = true;
        this.btnGetCookieIG.Click += new System.EventHandler(btnGetCookieIG_Click);
        this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        this.groupBox1.Controls.Add(this.btnLoadTKQC);
        this.groupBox1.Controls.Add(this.cbbTypeLoadTKQC);
        this.groupBox1.Controls.Add(this.btnLoadBM);
        this.groupBox1.Controls.Add(this.cbbTypeLoadBM);
        this.groupBox1.Location = new System.Drawing.Point(1197, 33);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(294, 437);
        this.groupBox1.TabIndex = 6;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Chức năng";
        this.btnLoadTKQC.Location = new System.Drawing.Point(7, 61);
        this.btnLoadTKQC.Name = "btnLoadTKQC";
        this.btnLoadTKQC.Size = new System.Drawing.Size(91, 23);
        this.btnLoadTKQC.TabIndex = 3;
        this.btnLoadTKQC.Text = "Load TKQC";
        this.btnLoadTKQC.UseVisualStyleBackColor = true;
        this.btnLoadTKQC.Click += new System.EventHandler(btnLoadTKQC_Click);
        this.cbbTypeLoadTKQC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeLoadTKQC.FormattingEnabled = true;
        this.cbbTypeLoadTKQC.Items.AddRange(new object[3] { "Load Tất Cả", "Load TKQC Live", "Load TKQC Cá Nhân" });
        this.cbbTypeLoadTKQC.Location = new System.Drawing.Point(104, 61);
        this.cbbTypeLoadTKQC.Name = "cbbTypeLoadTKQC";
        this.cbbTypeLoadTKQC.Size = new System.Drawing.Size(151, 24);
        this.cbbTypeLoadTKQC.TabIndex = 2;
        this.btnLoadBM.Location = new System.Drawing.Point(7, 22);
        this.btnLoadBM.Name = "btnLoadBM";
        this.btnLoadBM.Size = new System.Drawing.Size(91, 23);
        this.btnLoadBM.TabIndex = 1;
        this.btnLoadBM.Text = "Load BM";
        this.btnLoadBM.UseVisualStyleBackColor = true;
        this.btnLoadBM.Click += new System.EventHandler(btnLoadBM_Click);
        this.cbbTypeLoadBM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeLoadBM.FormattingEnabled = true;
        this.cbbTypeLoadBM.Items.AddRange(new object[7] { "Load All BM", "Load BM Live", "Load BM Die", "Load BM350 Live", "Load BM350 Die", "Load BM350", "Load BM50" });
        this.cbbTypeLoadBM.Location = new System.Drawing.Point(104, 22);
        this.cbbTypeLoadBM.Name = "cbbTypeLoadBM";
        this.cbbTypeLoadBM.Size = new System.Drawing.Size(151, 24);
        this.cbbTypeLoadBM.TabIndex = 0;
        this.nudCountThreads.Location = new System.Drawing.Point(313, 10);
        this.nudCountThreads.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
        this.nudCountThreads.Name = "nudCountThreads";
        this.nudCountThreads.Size = new System.Drawing.Size(53, 23);
        this.nudCountThreads.TabIndex = 5;
        this.nudCountThreads.Value = new decimal(new int[4] { 10, 0, 0, 0 });
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(245, 12);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(62, 16);
        this.label2.TabIndex = 4;
        this.label2.Text = "Số luồng:";
        this.btnStop.Location = new System.Drawing.Point(122, 6);
        this.btnStop.Name = "btnStop";
        this.btnStop.Size = new System.Drawing.Size(108, 28);
        this.btnStop.TabIndex = 3;
        this.btnStop.Text = "Stop";
        this.btnStop.UseVisualStyleBackColor = true;
        this.btnStop.Click += new System.EventHandler(btnStop_Click);
        this.btnStart.Location = new System.Drawing.Point(8, 6);
        this.btnStart.Name = "btnStart";
        this.btnStart.Size = new System.Drawing.Size(108, 28);
        this.btnStart.TabIndex = 2;
        this.btnStart.Text = "Start";
        this.btnStart.UseVisualStyleBackColor = true;
        this.btnStart.Click += new System.EventHandler(btnStart_Click);
        this.dtgvVia.AllowUserToAddRows = false;
        this.dtgvVia.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.dtgvVia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dtgvVia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dtgvVia.Columns.AddRange(this.cStt, this.cUid, this.cPassword, this.cKey2FA, this.cCookie, this.cToken, this.cProcess);
        this.dtgvVia.ContextMenuStrip = this.ctmnsVia;
        this.dtgvVia.Location = new System.Drawing.Point(8, 40);
        this.dtgvVia.Name = "dtgvVia";
        this.dtgvVia.RowHeadersVisible = false;
        this.dtgvVia.Size = new System.Drawing.Size(1183, 430);
        this.dtgvVia.TabIndex = 1;
        this.cStt.FillWeight = 30f;
        this.cStt.HeaderText = "Stt";
        this.cStt.Name = "cStt";
        this.cUid.FillWeight = 50f;
        this.cUid.HeaderText = "Uid";
        this.cUid.Name = "cUid";
        this.cPassword.HeaderText = "Password";
        this.cPassword.Name = "cPassword";
        this.cKey2FA.FillWeight = 45f;
        this.cKey2FA.HeaderText = "Key2FA";
        this.cKey2FA.Name = "cKey2FA";
        this.cCookie.HeaderText = "Cookie";
        this.cCookie.Name = "cCookie";
        this.cToken.HeaderText = "Token";
        this.cToken.Name = "cToken";
        this.cProcess.HeaderText = "Process";
        this.cProcess.Name = "cProcess";
        this.ctmnsVia.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.ctmnsVia.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.dánViaToolStripMenuItem, this.xóaViaToolStripMenuItem });
        this.ctmnsVia.Name = "ctmnsVia";
        this.ctmnsVia.Size = new System.Drawing.Size(115, 48);
        this.dánViaToolStripMenuItem.Name = "dánViaToolStripMenuItem";
        this.dánViaToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
        this.dánViaToolStripMenuItem.Text = "Dán Via";
        this.dánViaToolStripMenuItem.Click += new System.EventHandler(dánViaToolStripMenuItem_Click);
        this.xóaViaToolStripMenuItem.Name = "xóaViaToolStripMenuItem";
        this.xóaViaToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
        this.xóaViaToolStripMenuItem.Text = "Xóa Via";
        this.xóaViaToolStripMenuItem.Click += new System.EventHandler(xóaViaToolStripMenuItem_Click);
        this.tabPage5.Controls.Add(this.label27);
        this.tabPage5.Controls.Add(this.tbProxy);
        this.tabPage5.Controls.Add(this.label5);
        this.tabPage5.Controls.Add(this.rtbLog);
        this.tabPage5.Controls.Add(this.label4);
        this.tabPage5.Controls.Add(this.textBox1);
        this.tabPage5.Controls.Add(this.label3);
        this.tabPage5.Controls.Add(this.cbbTypeProxy);
        this.tabPage5.Controls.Add(this.label1);
        this.tabPage5.Controls.Add(this.cbbTypeLogin);
        this.tabPage5.Location = new System.Drawing.Point(4, 25);
        this.tabPage5.Name = "tabPage5";
        this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage5.Size = new System.Drawing.Size(1497, 476);
        this.tabPage5.TabIndex = 1;
        this.tabPage5.Text = "Cấu hình";
        this.tabPage5.UseVisualStyleBackColor = true;
        this.label27.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.label27.AutoSize = true;
        this.label27.Location = new System.Drawing.Point(266, 59);
        this.label27.Name = "label27";
        this.label27.Size = new System.Drawing.Size(43, 16);
        this.label27.TabIndex = 11;
        this.label27.Text = "Proxy:";
        this.tbProxy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.tbProxy.Location = new System.Drawing.Point(354, 56);
        this.tbProxy.Name = "tbProxy";
        this.tbProxy.Size = new System.Drawing.Size(223, 23);
        this.tbProxy.TabIndex = 10;
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(6, 97);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(32, 16);
        this.label5.TabIndex = 9;
        this.label5.Text = "Log:";
        this.rtbLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.rtbLog.Location = new System.Drawing.Point(9, 116);
        this.rtbLog.Name = "rtbLog";
        this.rtbLog.Size = new System.Drawing.Size(339, 522);
        this.rtbLog.TabIndex = 8;
        this.rtbLog.Text = "";
        this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(266, 26);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(82, 16);
        this.label4.TabIndex = 7;
        this.label4.Text = "Key Captcha:";
        this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.textBox1.Location = new System.Drawing.Point(354, 23);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(223, 23);
        this.textBox1.TabIndex = 6;
        this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(6, 56);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(43, 16);
        this.label3.TabIndex = 5;
        this.label3.Text = "Proxy:";
        this.cbbTypeProxy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.cbbTypeProxy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeProxy.FormattingEnabled = true;
        this.cbbTypeProxy.Items.AddRange(new object[2] { "None", "IP:PORT" });
        this.cbbTypeProxy.Location = new System.Drawing.Point(81, 53);
        this.cbbTypeProxy.Name = "cbbTypeProxy";
        this.cbbTypeProxy.Size = new System.Drawing.Size(161, 24);
        this.cbbTypeProxy.TabIndex = 4;
        this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(6, 26);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(69, 16);
        this.label1.TabIndex = 1;
        this.label1.Text = "Loại Login:";
        this.cbbTypeLogin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        this.cbbTypeLogin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeLogin.FormattingEnabled = true;
        this.cbbTypeLogin.Items.AddRange(new object[2] { "Cookie", "Uid|Pass" });
        this.cbbTypeLogin.Location = new System.Drawing.Point(81, 23);
        this.cbbTypeLogin.Name = "cbbTypeLogin";
        this.cbbTypeLogin.Size = new System.Drawing.Size(161, 24);
        this.cbbTypeLogin.TabIndex = 0;
        this.tsVia.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.tsVia.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.tsVia.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.lblStatus, this.lblTextTotalCount, this.lblTotalCount, this.toolStripLabel1, this.lblCountSelected });
        this.tsVia.Location = new System.Drawing.Point(3, 508);
        this.tsVia.Name = "tsVia";
        this.tsVia.Size = new System.Drawing.Size(1505, 25);
        this.tsVia.TabIndex = 0;
        this.tsVia.Text = "toolStrip1";
        this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.lblStatus.ForeColor = System.Drawing.Color.Red;
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(109, 22);
        this.lblStatus.Text = "Status: Stopped";
        this.lblTextTotalCount.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
        this.lblTextTotalCount.Name = "lblTextTotalCount";
        this.lblTextTotalCount.Size = new System.Drawing.Size(42, 22);
        this.lblTextTotalCount.Text = "Tổng:";
        this.lblTotalCount.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
        this.lblTotalCount.ForeColor = System.Drawing.Color.DodgerBlue;
        this.lblTotalCount.Name = "lblTotalCount";
        this.lblTotalCount.Size = new System.Drawing.Size(15, 22);
        this.lblTotalCount.Text = "0";
        this.toolStripLabel1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
        this.toolStripLabel1.Name = "toolStripLabel1";
        this.toolStripLabel1.Size = new System.Drawing.Size(57, 22);
        this.toolStripLabel1.Text = "Bôi đen:";
        this.lblCountSelected.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
        this.lblCountSelected.ForeColor = System.Drawing.Color.DarkGreen;
        this.lblCountSelected.Name = "lblCountSelected";
        this.lblCountSelected.Size = new System.Drawing.Size(15, 22);
        this.lblCountSelected.Text = "0";
        this.tabPage2.Controls.Add(this.groupBox2);
        this.tabPage2.Controls.Add(this.tsBM);
        this.tabPage2.Controls.Add(this.dtgvBM);
        this.tabPage2.Location = new System.Drawing.Point(4, 25);
        this.tabPage2.Name = "tabPage2";
        this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage2.Size = new System.Drawing.Size(1511, 536);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text = "Cấu Hình BM";
        this.tabPage2.UseVisualStyleBackColor = true;
        this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.groupBox2.Controls.Add(this.tabControl3);
        this.groupBox2.Location = new System.Drawing.Point(8, 6);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(1495, 203);
        this.groupBox2.TabIndex = 2;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Chức năng";
        this.tabControl3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.tabControl3.Controls.Add(this.tabPage7);
        this.tabControl3.Controls.Add(this.tabPage8);
        this.tabControl3.Controls.Add(this.tabPage11);
        this.tabControl3.Controls.Add(this.tabPage10);
        this.tabControl3.Controls.Add(this.tabPage12);
        this.tabControl3.Controls.Add(this.tabPage14);
        this.tabControl3.Location = new System.Drawing.Point(6, 22);
        this.tabControl3.Name = "tabControl3";
        this.tabControl3.SelectedIndex = 0;
        this.tabControl3.Size = new System.Drawing.Size(1483, 172);
        this.tabControl3.TabIndex = 4;
        this.tabPage7.Controls.Add(this.groupBox3);
        this.tabPage7.Location = new System.Drawing.Point(4, 25);
        this.tabPage7.Name = "tabPage7";
        this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage7.Size = new System.Drawing.Size(1475, 143);
        this.tabPage7.TabIndex = 0;
        this.tabPage7.Text = "BackUp";
        this.tabPage7.UseVisualStyleBackColor = true;
        this.groupBox3.Controls.Add(this.label21);
        this.groupBox3.Controls.Add(this.tbMailPass);
        this.groupBox3.Controls.Add(this.tableLayoutPanel1);
        this.groupBox3.Controls.Add(this.lblCountLinkBM);
        this.groupBox3.Controls.Add(this.rtbLinkBM);
        this.groupBox3.Controls.Add(this.label10);
        this.groupBox3.Location = new System.Drawing.Point(6, 9);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(1463, 128);
        this.groupBox3.TabIndex = 2;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Share BM";
        this.label21.AutoSize = true;
        this.label21.Location = new System.Drawing.Point(265, 29);
        this.label21.Name = "label21";
        this.label21.Size = new System.Drawing.Size(43, 16);
        this.label21.TabIndex = 5;
        this.label21.Text = "Email:";
        this.tbMailPass.Location = new System.Drawing.Point(314, 26);
        this.tbMailPass.Name = "tbMailPass";
        this.tbMailPass.Size = new System.Drawing.Size(190, 23);
        this.tbMailPass.TabIndex = 9;
        this.tableLayoutPanel1.ColumnCount = 2;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel1.Controls.Add(this.btnOutBM, 1, 2);
        this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.cbbTypeMail, 1, 0);
        this.tableLayoutPanel1.Controls.Add(this.cbbTypeRole, 1, 1);
        this.tableLayoutPanel1.Controls.Add(this.btnBackUp, 0, 2);
        this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
        this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 22);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 4;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(253, 100);
        this.tableLayoutPanel1.TabIndex = 8;
        this.btnOutBM.Location = new System.Drawing.Point(84, 63);
        this.btnOutBM.Name = "btnOutBM";
        this.btnOutBM.Size = new System.Drawing.Size(84, 23);
        this.btnOutBM.TabIndex = 0;
        this.btnOutBM.Text = "Thoát";
        this.btnOutBM.UseVisualStyleBackColor = true;
        this.btnOutBM.Click += new System.EventHandler(btnOutBM_Click);
        this.label7.AutoSize = true;
        this.label7.Location = new System.Drawing.Point(3, 30);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(48, 16);
        this.label7.TabIndex = 3;
        this.label7.Text = "Quyền:";
        this.cbbTypeMail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeMail.FormattingEnabled = true;
        this.cbbTypeMail.Items.AddRange(new object[3] { "Moakt", "Mailngon.top", "fviainboxes" });
        this.cbbTypeMail.Location = new System.Drawing.Point(84, 3);
        this.cbbTypeMail.Name = "cbbTypeMail";
        this.cbbTypeMail.Size = new System.Drawing.Size(154, 24);
        this.cbbTypeMail.TabIndex = 0;
        this.cbbTypeRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeRole.Enabled = false;
        this.cbbTypeRole.FormattingEnabled = true;
        this.cbbTypeRole.Items.AddRange(new object[2] { "ADMIN", "EMPLOYEE" });
        this.cbbTypeRole.Location = new System.Drawing.Point(84, 33);
        this.cbbTypeRole.Name = "cbbTypeRole";
        this.cbbTypeRole.Size = new System.Drawing.Size(154, 24);
        this.cbbTypeRole.TabIndex = 2;
        this.btnBackUp.Location = new System.Drawing.Point(3, 63);
        this.btnBackUp.Name = "btnBackUp";
        this.btnBackUp.Size = new System.Drawing.Size(75, 23);
        this.btnBackUp.TabIndex = 4;
        this.btnBackUp.Text = "BackUp";
        this.btnBackUp.UseVisualStyleBackColor = true;
        this.btnBackUp.Click += new System.EventHandler(btnBackUp_Click);
        this.label6.AutoSize = true;
        this.label6.Location = new System.Drawing.Point(3, 0);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(43, 16);
        this.label6.TabIndex = 1;
        this.label6.Text = "Email:";
        this.lblCountLinkBM.AutoSize = true;
        this.lblCountLinkBM.BackColor = System.Drawing.Color.Green;
        this.lblCountLinkBM.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.lblCountLinkBM.ForeColor = System.Drawing.SystemColors.Control;
        this.lblCountLinkBM.Location = new System.Drawing.Point(579, 18);
        this.lblCountLinkBM.Name = "lblCountLinkBM";
        this.lblCountLinkBM.Size = new System.Drawing.Size(15, 16);
        this.lblCountLinkBM.TabIndex = 7;
        this.lblCountLinkBM.Text = "0";
        this.rtbLinkBM.Location = new System.Drawing.Point(540, 35);
        this.rtbLinkBM.Name = "rtbLinkBM";
        this.rtbLinkBM.Size = new System.Drawing.Size(812, 87);
        this.rtbLinkBM.TabIndex = 5;
        this.rtbLinkBM.Text = "";
        this.rtbLinkBM.TextChanged += new System.EventHandler(rtbLinkBM_TextChanged);
        this.label10.AutoSize = true;
        this.label10.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label10.Location = new System.Drawing.Point(537, 19);
        this.label10.Name = "label10";
        this.label10.Size = new System.Drawing.Size(36, 14);
        this.label10.TabIndex = 6;
        this.label10.Text = "Link:";
        this.tabPage8.Controls.Add(this.groupBox4);
        this.tabPage8.Location = new System.Drawing.Point(4, 25);
        this.tabPage8.Name = "tabPage8";
        this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage8.Size = new System.Drawing.Size(1475, 143);
        this.tabPage8.TabIndex = 1;
        this.tabPage8.Text = "Tạo TKQC";
        this.tabPage8.UseVisualStyleBackColor = true;
        this.groupBox4.Controls.Add(this.label19);
        this.groupBox4.Controls.Add(this.nudCountNumAdAccount);
        this.groupBox4.Controls.Add(this.tbNameBM);
        this.groupBox4.Controls.Add(this.label14);
        this.groupBox4.Controls.Add(this.tbRegion);
        this.groupBox4.Controls.Add(this.label8);
        this.groupBox4.Controls.Add(this.label13);
        this.groupBox4.Controls.Add(this.cbbCurrencyBM);
        this.groupBox4.Controls.Add(this.btnCreateAdAccount);
        this.groupBox4.Controls.Add(this.cbbTimeZoneBM);
        this.groupBox4.Controls.Add(this.label12);
        this.groupBox4.Controls.Add(this.label9);
        this.groupBox4.Controls.Add(this.label11);
        this.groupBox4.Controls.Add(this.tbPartner);
        this.groupBox4.Controls.Add(this.cbbTypeCreateAdAccount);
        this.groupBox4.Location = new System.Drawing.Point(6, 6);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.Size = new System.Drawing.Size(1390, 146);
        this.groupBox4.TabIndex = 3;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "Tạo TKQC";
        this.label19.AutoSize = true;
        this.label19.Location = new System.Drawing.Point(129, 85);
        this.label19.Name = "label19";
        this.label19.Size = new System.Drawing.Size(63, 16);
        this.label19.TabIndex = 19;
        this.label19.Text = "Số lượng:";
        this.nudCountNumAdAccount.Location = new System.Drawing.Point(198, 83);
        this.nudCountNumAdAccount.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
        this.nudCountNumAdAccount.Name = "nudCountNumAdAccount";
        this.nudCountNumAdAccount.Size = new System.Drawing.Size(40, 23);
        this.nudCountNumAdAccount.TabIndex = 18;
        this.nudCountNumAdAccount.Value = new decimal(new int[4] { 1, 0, 0, 0 });
        this.tbNameBM.Location = new System.Drawing.Point(498, 85);
        this.tbNameBM.Name = "tbNameBM";
        this.tbNameBM.Size = new System.Drawing.Size(112, 23);
        this.tbNameBM.TabIndex = 16;
        this.tbNameBM.Text = "ADS";
        this.label14.AutoSize = true;
        this.label14.Location = new System.Drawing.Point(458, 88);
        this.label14.Name = "label14";
        this.label14.Size = new System.Drawing.Size(34, 16);
        this.label14.TabIndex = 17;
        this.label14.Text = "Tên:";
        this.tbRegion.Location = new System.Drawing.Point(312, 85);
        this.tbRegion.Name = "tbRegion";
        this.tbRegion.Size = new System.Drawing.Size(91, 23);
        this.tbRegion.TabIndex = 12;
        this.tbRegion.Text = "US";
        this.label8.AutoSize = true;
        this.label8.Location = new System.Drawing.Point(6, 25);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(73, 16);
        this.label8.TabIndex = 6;
        this.label8.Text = "Chức năng:";
        this.label13.AutoSize = true;
        this.label13.Location = new System.Drawing.Point(244, 88);
        this.label13.Name = "label13";
        this.label13.Size = new System.Drawing.Size(62, 16);
        this.label13.TabIndex = 15;
        this.label13.Text = "Quốc gia:";
        this.cbbCurrencyBM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbCurrencyBM.FormattingEnabled = true;
        this.cbbCurrencyBM.Items.AddRange(new object[2] { "Tạo TKQC", "Tạo TKQC Share đối tác" });
        this.cbbCurrencyBM.Location = new System.Drawing.Point(312, 53);
        this.cbbCurrencyBM.Name = "cbbCurrencyBM";
        this.cbbCurrencyBM.Size = new System.Drawing.Size(298, 24);
        this.cbbCurrencyBM.TabIndex = 13;
        this.btnCreateAdAccount.Location = new System.Drawing.Point(9, 85);
        this.btnCreateAdAccount.Name = "btnCreateAdAccount";
        this.btnCreateAdAccount.Size = new System.Drawing.Size(75, 23);
        this.btnCreateAdAccount.TabIndex = 9;
        this.btnCreateAdAccount.Text = "Start";
        this.btnCreateAdAccount.UseVisualStyleBackColor = true;
        this.btnCreateAdAccount.Click += new System.EventHandler(btnCreateAdAccount_Click);
        this.cbbTimeZoneBM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTimeZoneBM.FormattingEnabled = true;
        this.cbbTimeZoneBM.Items.AddRange(new object[2] { "Tạo TKQC", "Tạo TKQC Share đối tác" });
        this.cbbTimeZoneBM.Location = new System.Drawing.Point(312, 22);
        this.cbbTimeZoneBM.Name = "cbbTimeZoneBM";
        this.cbbTimeZoneBM.Size = new System.Drawing.Size(298, 24);
        this.cbbTimeZoneBM.TabIndex = 12;
        this.label12.AutoSize = true;
        this.label12.Location = new System.Drawing.Point(244, 56);
        this.label12.Name = "label12";
        this.label12.Size = new System.Drawing.Size(52, 16);
        this.label12.TabIndex = 14;
        this.label12.Text = "Tiền tệ:";
        this.label9.AutoSize = true;
        this.label9.Location = new System.Drawing.Point(6, 56);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(52, 16);
        this.label9.TabIndex = 8;
        this.label9.Text = "Đối tác:";
        this.label11.AutoSize = true;
        this.label11.Location = new System.Drawing.Point(244, 25);
        this.label11.Name = "label11";
        this.label11.Size = new System.Drawing.Size(53, 16);
        this.label11.TabIndex = 12;
        this.label11.Text = "Múi giờ:";
        this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.tbPartner.Location = new System.Drawing.Point(84, 53);
        this.tbPartner.Name = "tbPartner";
        this.tbPartner.Size = new System.Drawing.Size(154, 23);
        this.tbPartner.TabIndex = 7;
        this.cbbTypeCreateAdAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTypeCreateAdAccount.FormattingEnabled = true;
        this.cbbTypeCreateAdAccount.Items.AddRange(new object[2] { "Tạo TKQC", "Tạo TKQC Share đối tác" });
        this.cbbTypeCreateAdAccount.Location = new System.Drawing.Point(84, 22);
        this.cbbTypeCreateAdAccount.Name = "cbbTypeCreateAdAccount";
        this.cbbTypeCreateAdAccount.Size = new System.Drawing.Size(154, 24);
        this.cbbTypeCreateAdAccount.TabIndex = 5;
        this.tabPage11.Controls.Add(this.btnChangeEmail);
        this.tabPage11.Controls.Add(this.label20);
        this.tabPage11.Controls.Add(this.tbMailBM);
        this.tabPage11.Controls.Add(this.btnChangeNameBM);
        this.tabPage11.Controls.Add(this.label17);
        this.tabPage11.Controls.Add(this.tbBusinessName);
        this.tabPage11.Location = new System.Drawing.Point(4, 25);
        this.tabPage11.Name = "tabPage11";
        this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage11.Size = new System.Drawing.Size(1475, 143);
        this.tabPage11.TabIndex = 2;
        this.tabPage11.Text = "Đổi tên BM";
        this.tabPage11.UseVisualStyleBackColor = true;
        this.btnChangeEmail.Location = new System.Drawing.Point(266, 44);
        this.btnChangeEmail.Name = "btnChangeEmail";
        this.btnChangeEmail.Size = new System.Drawing.Size(75, 23);
        this.btnChangeEmail.TabIndex = 5;
        this.btnChangeEmail.Text = "Đổi";
        this.btnChangeEmail.UseVisualStyleBackColor = true;
        this.btnChangeEmail.Click += new System.EventHandler(btnChangeEmail_Click);
        this.label20.AutoSize = true;
        this.label20.Location = new System.Drawing.Point(7, 44);
        this.label20.Name = "label20";
        this.label20.Size = new System.Drawing.Size(43, 16);
        this.label20.TabIndex = 4;
        this.label20.Text = "Email:";
        this.tbMailBM.Location = new System.Drawing.Point(68, 41);
        this.tbMailBM.Name = "tbMailBM";
        this.tbMailBM.Size = new System.Drawing.Size(192, 23);
        this.tbMailBM.TabIndex = 3;
        this.tbMailBM.Text = "nbteamads2019@gmail.com";
        this.btnChangeNameBM.Location = new System.Drawing.Point(266, 12);
        this.btnChangeNameBM.Name = "btnChangeNameBM";
        this.btnChangeNameBM.Size = new System.Drawing.Size(75, 23);
        this.btnChangeNameBM.TabIndex = 2;
        this.btnChangeNameBM.Text = "Đổi";
        this.btnChangeNameBM.UseVisualStyleBackColor = true;
        this.btnChangeNameBM.Click += new System.EventHandler(btnChangeNameBM_Click);
        this.label17.AutoSize = true;
        this.label17.Location = new System.Drawing.Point(7, 15);
        this.label17.Name = "label17";
        this.label17.Size = new System.Drawing.Size(55, 16);
        this.label17.TabIndex = 1;
        this.label17.Text = "Tên BM:";
        this.tbBusinessName.Location = new System.Drawing.Point(68, 12);
        this.tbBusinessName.Name = "tbBusinessName";
        this.tbBusinessName.Size = new System.Drawing.Size(192, 23);
        this.tbBusinessName.TabIndex = 0;
        this.tbBusinessName.Text = "NBTeam Ads";
        this.tabPage10.Controls.Add(this.btnJoinBM);
        this.tabPage10.Controls.Add(this.tbLinkBM);
        this.tabPage10.Location = new System.Drawing.Point(4, 25);
        this.tabPage10.Name = "tabPage10";
        this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage10.Size = new System.Drawing.Size(1475, 143);
        this.tabPage10.TabIndex = 4;
        this.tabPage10.Text = "Nhận Link BM";
        this.tabPage10.UseVisualStyleBackColor = true;
        this.btnJoinBM.Location = new System.Drawing.Point(939, 6);
        this.btnJoinBM.Name = "btnJoinBM";
        this.btnJoinBM.Size = new System.Drawing.Size(80, 27);
        this.btnJoinBM.TabIndex = 1;
        this.btnJoinBM.Text = "Nhận Link";
        this.btnJoinBM.UseVisualStyleBackColor = true;
        this.btnJoinBM.Click += new System.EventHandler(btnJoinBM_Click);
        this.tbLinkBM.Location = new System.Drawing.Point(6, 6);
        this.tbLinkBM.Multiline = true;
        this.tbLinkBM.Name = "tbLinkBM";
        this.tbLinkBM.Size = new System.Drawing.Size(927, 131);
        this.tbLinkBM.TabIndex = 0;
        this.tabPage12.Controls.Add(this.label23);
        this.tabPage12.Controls.Add(this.tbViotpApiKey);
        this.tabPage12.Controls.Add(this.btnAppeal);
        this.tabPage12.Controls.Add(this.label22);
        this.tabPage12.Controls.Add(this.tbCaptchaKey);
        this.tabPage12.Location = new System.Drawing.Point(4, 25);
        this.tabPage12.Name = "tabPage12";
        this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage12.Size = new System.Drawing.Size(1475, 143);
        this.tabPage12.TabIndex = 5;
        this.tabPage12.Text = "Kháng BM";
        this.tabPage12.UseVisualStyleBackColor = true;
        this.label23.AutoSize = true;
        this.label23.Location = new System.Drawing.Point(7, 42);
        this.label23.Name = "label23";
        this.label23.Size = new System.Drawing.Size(63, 16);
        this.label23.TabIndex = 4;
        this.label23.Text = "Api Viotp:";
        this.tbViotpApiKey.Location = new System.Drawing.Point(95, 39);
        this.tbViotpApiKey.Name = "tbViotpApiKey";
        this.tbViotpApiKey.Size = new System.Drawing.Size(176, 23);
        this.tbViotpApiKey.TabIndex = 3;
        this.btnAppeal.Location = new System.Drawing.Point(192, 68);
        this.btnAppeal.Name = "btnAppeal";
        this.btnAppeal.Size = new System.Drawing.Size(79, 26);
        this.btnAppeal.TabIndex = 2;
        this.btnAppeal.Text = "Kháng";
        this.btnAppeal.UseVisualStyleBackColor = true;
        this.btnAppeal.Click += new System.EventHandler(btnAppel_Click);
        this.label22.AutoSize = true;
        this.label22.Location = new System.Drawing.Point(7, 13);
        this.label22.Name = "label22";
        this.label22.Size = new System.Drawing.Size(82, 16);
        this.label22.TabIndex = 1;
        this.label22.Text = "Captcha Key:";
        this.tbCaptchaKey.Location = new System.Drawing.Point(95, 10);
        this.tbCaptchaKey.Name = "tbCaptchaKey";
        this.tbCaptchaKey.Size = new System.Drawing.Size(176, 23);
        this.tbCaptchaKey.TabIndex = 0;
        this.tabPage14.Controls.Add(this.btnCreateWA);
        this.tabPage14.Controls.Add(this.label24);
        this.tabPage14.Controls.Add(this.nudCountCreateWA);
        this.tabPage14.Location = new System.Drawing.Point(4, 25);
        this.tabPage14.Name = "tabPage14";
        this.tabPage14.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage14.Size = new System.Drawing.Size(1475, 143);
        this.tabPage14.TabIndex = 6;
        this.tabPage14.Text = "Tạo WA";
        this.tabPage14.UseVisualStyleBackColor = true;
        this.btnCreateWA.Location = new System.Drawing.Point(122, 14);
        this.btnCreateWA.Name = "btnCreateWA";
        this.btnCreateWA.Size = new System.Drawing.Size(84, 23);
        this.btnCreateWA.TabIndex = 22;
        this.btnCreateWA.Text = "Tạo WA";
        this.btnCreateWA.UseVisualStyleBackColor = true;
        this.btnCreateWA.Click += new System.EventHandler(btnCreateWA_Click);
        this.label24.AutoSize = true;
        this.label24.Location = new System.Drawing.Point(7, 17);
        this.label24.Name = "label24";
        this.label24.Size = new System.Drawing.Size(63, 16);
        this.label24.TabIndex = 21;
        this.label24.Text = "Số lượng:";
        this.nudCountCreateWA.Location = new System.Drawing.Point(76, 15);
        this.nudCountCreateWA.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
        this.nudCountCreateWA.Name = "nudCountCreateWA";
        this.nudCountCreateWA.Size = new System.Drawing.Size(40, 23);
        this.nudCountCreateWA.TabIndex = 20;
        this.nudCountCreateWA.Value = new decimal(new int[4] { 1, 0, 0, 0 });
        this.tsBM.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.tsBM.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.tsBM.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripLabel2, this.lblCountTotalBM });
        this.tsBM.Location = new System.Drawing.Point(3, 508);
        this.tsBM.Name = "tsBM";
        this.tsBM.Size = new System.Drawing.Size(1505, 25);
        this.tsBM.TabIndex = 1;
        this.tsBM.Text = "toolStrip1";
        this.toolStripLabel2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
        this.toolStripLabel2.Name = "toolStripLabel2";
        this.toolStripLabel2.Size = new System.Drawing.Size(42, 22);
        this.toolStripLabel2.Text = "Tổng:";
        this.lblCountTotalBM.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
        this.lblCountTotalBM.ForeColor = System.Drawing.Color.Green;
        this.lblCountTotalBM.Name = "lblCountTotalBM";
        this.lblCountTotalBM.Size = new System.Drawing.Size(15, 22);
        this.lblCountTotalBM.Text = "0";
        this.dtgvBM.AllowUserToAddRows = false;
        this.dtgvBM.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.dtgvBM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dtgvBM.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
        this.dtgvBM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dtgvBM.Columns.AddRange(this.cSttBM, this.cIDBMVia, this.cUidVia, this.cNameBM, this.cBMType, this.cVerifyBM, this.cCreateAdAccountBM, this.cCountQTVBM, this.cInfoBM, this.cCreateTimeBM, this.cStatusBM, this.cLimitAdAccountBM, this.cAdAccountCount, this.cQTVBM, this.cProcessBM);
        this.dtgvBM.ContextMenuStrip = this.ctmnsBM;
        this.dtgvBM.Location = new System.Drawing.Point(3, 215);
        this.dtgvBM.Name = "dtgvBM";
        this.dtgvBM.RowHeadersVisible = false;
        this.dtgvBM.Size = new System.Drawing.Size(1500, 290);
        this.dtgvBM.TabIndex = 0;
        this.dtgvBM.KeyDown += new System.Windows.Forms.KeyEventHandler(dtgvBM_KeyDown);
        this.cSttBM.FillWeight = 30f;
        this.cSttBM.HeaderText = "Stt";
        this.cSttBM.Name = "cSttBM";
        this.cIDBMVia.HeaderText = "ID BM";
        this.cIDBMVia.Name = "cIDBMVia";
        this.cUidVia.HeaderText = "Uid Via";
        this.cUidVia.Name = "cUidVia";
        this.cNameBM.HeaderText = "Name BM";
        this.cNameBM.Name = "cNameBM";
        this.cBMType.FillWeight = 30f;
        this.cBMType.HeaderText = "BM Type";
        this.cBMType.Name = "cBMType";
        this.cVerifyBM.FillWeight = 30f;
        this.cVerifyBM.HeaderText = "XMDN";
        this.cVerifyBM.Name = "cVerifyBM";
        this.cCreateAdAccountBM.HeaderText = "Create Ad Account";
        this.cCreateAdAccountBM.Name = "cCreateAdAccountBM";
        this.cCountQTVBM.FillWeight = 45f;
        this.cCountQTVBM.HeaderText = "SL QTV";
        this.cCountQTVBM.Name = "cCountQTVBM";
        this.cInfoBM.FillWeight = 45f;
        this.cInfoBM.HeaderText = "Info BM";
        this.cInfoBM.Name = "cInfoBM";
        this.cCreateTimeBM.HeaderText = "Create Time";
        this.cCreateTimeBM.Name = "cCreateTimeBM";
        this.cStatusBM.HeaderText = "Status";
        this.cStatusBM.Name = "cStatusBM";
        this.cLimitAdAccountBM.FillWeight = 45f;
        this.cLimitAdAccountBM.HeaderText = "Limit BM";
        this.cLimitAdAccountBM.Name = "cLimitAdAccountBM";
        this.cAdAccountCount.FillWeight = 45f;
        this.cAdAccountCount.HeaderText = "SL TKQC";
        this.cAdAccountCount.Name = "cAdAccountCount";
        this.cQTVBM.HeaderText = "QTV";
        this.cQTVBM.Name = "cQTVBM";
        this.cProcessBM.HeaderText = "Process";
        this.cProcessBM.Name = "cProcessBM";
        this.ctmnsBM.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.ctmnsBM.Items.AddRange(new System.Windows.Forms.ToolStripItem[20]
        {
            this.dánIDToolStripMenuItem, this.loadTKQCToolStripMenuItem, this.checkInfoToolStripMenuItem, this.checkInfoBMToolStripMenuItem, this.checkLimitBMToolStripMenuItem, this.checkBMToolStripMenuItem, this.checkQTVBMToolStripMenuItem, this.checkWAToolStripMenuItem, this.loadUserToolStripMenuItem, this.loadAssetToolStripMenuItem,
            this.mởBMToolStripMenuItem, this.kíchAppToolStripMenuItem, this.kíchAppToolStripMenuItem1, this.kíchAppIGToolStripMenuItem, this.kíchNútKhángToolStripMenuItem, this.tạoWAToolStripMenuItem, this.tạoTKQCToolStripMenuItem, this.xóaWAToolStripMenuItem, this.checkLimitTKẨnToolStripMenuItem, this.mởBMChromeToolStripMenuItem
        });
        this.ctmnsBM.Name = "ctmnsBM";
        this.ctmnsBM.Size = new System.Drawing.Size(201, 444);
        this.dánIDToolStripMenuItem.Name = "dánIDToolStripMenuItem";
        this.dánIDToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.dánIDToolStripMenuItem.Text = "Dán ID";
        this.dánIDToolStripMenuItem.Click += new System.EventHandler(dánIDToolStripMenuItem_Click);
        this.loadTKQCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.loadLiveToolStripMenuItem, this.loadDieToolStripMenuItem, this.loadTKẨnToolStripMenuItem });
        this.loadTKQCToolStripMenuItem.Name = "loadTKQCToolStripMenuItem";
        this.loadTKQCToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.loadTKQCToolStripMenuItem.Text = "Load TKQC";
        this.loadTKQCToolStripMenuItem.Click += new System.EventHandler(loadTKQCToolStripMenuItem_Click);
        this.loadLiveToolStripMenuItem.Name = "loadLiveToolStripMenuItem";
        this.loadLiveToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
        this.loadLiveToolStripMenuItem.Text = "Load Live";
        this.loadLiveToolStripMenuItem.Click += new System.EventHandler(loadLiveToolStripMenuItem_Click);
        this.loadDieToolStripMenuItem.Name = "loadDieToolStripMenuItem";
        this.loadDieToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
        this.loadDieToolStripMenuItem.Text = "Load Die";
        this.loadDieToolStripMenuItem.Click += new System.EventHandler(loadDieToolStripMenuItem_Click);
        this.loadTKẨnToolStripMenuItem.Name = "loadTKẨnToolStripMenuItem";
        this.loadTKẨnToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
        this.loadTKẨnToolStripMenuItem.Text = "Load TK ẩn";
        this.loadTKẨnToolStripMenuItem.Click += new System.EventHandler(loadTKẨnToolStripMenuItem_Click);
        this.checkInfoToolStripMenuItem.Name = "checkInfoToolStripMenuItem";
        this.checkInfoToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkInfoToolStripMenuItem.Text = "Check Full BM";
        this.checkInfoToolStripMenuItem.Click += new System.EventHandler(checkInfoToolStripMenuItem_Click);
        this.checkInfoBMToolStripMenuItem.Name = "checkInfoBMToolStripMenuItem";
        this.checkInfoBMToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkInfoBMToolStripMenuItem.Text = "Check Info BM";
        this.checkInfoBMToolStripMenuItem.Click += new System.EventHandler(checkInfoBMToolStripMenuItem_Click);
        this.checkLimitBMToolStripMenuItem.Name = "checkLimitBMToolStripMenuItem";
        this.checkLimitBMToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkLimitBMToolStripMenuItem.Text = "Check Limit BM";
        this.checkLimitBMToolStripMenuItem.Click += new System.EventHandler(checkLimitBMToolStripMenuItem_Click);
        this.checkBMToolStripMenuItem.Name = "checkBMToolStripMenuItem";
        this.checkBMToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkBMToolStripMenuItem.Text = "Check BM Die";
        this.checkBMToolStripMenuItem.Click += new System.EventHandler(checkBMToolStripMenuItem_Click);
        this.checkQTVBMToolStripMenuItem.Name = "checkQTVBMToolStripMenuItem";
        this.checkQTVBMToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkQTVBMToolStripMenuItem.Text = "Check QTV BM";
        this.checkQTVBMToolStripMenuItem.Click += new System.EventHandler(checkQTVBMToolStripMenuItem_Click);
        this.checkWAToolStripMenuItem.Name = "checkWAToolStripMenuItem";
        this.checkWAToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkWAToolStripMenuItem.Text = "Check WA";
        this.checkWAToolStripMenuItem.Click += new System.EventHandler(checkWAToolStripMenuItem_Click);
        this.loadUserToolStripMenuItem.Name = "loadUserToolStripMenuItem";
        this.loadUserToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.loadUserToolStripMenuItem.Text = "Load User";
        this.loadUserToolStripMenuItem.Click += new System.EventHandler(loadUserToolStripMenuItem_Click);
        this.loadAssetToolStripMenuItem.Name = "loadAssetToolStripMenuItem";
        this.loadAssetToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.loadAssetToolStripMenuItem.Text = "Load Asset";
        this.mởBMToolStripMenuItem.Name = "mởBMToolStripMenuItem";
        this.mởBMToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.mởBMToolStripMenuItem.Text = "Mở BM Chrome";
        this.mởBMToolStripMenuItem.Click += new System.EventHandler(mởBMToolStripMenuItem_Click);
        this.kíchAppToolStripMenuItem.Name = "kíchAppToolStripMenuItem";
        this.kíchAppToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.kíchAppToolStripMenuItem.Text = "Kích App + TẠO WA";
        this.kíchAppToolStripMenuItem.Click += new System.EventHandler(kíchAppToolStripMenuItem_Click);
        this.kíchAppToolStripMenuItem1.Name = "kíchAppToolStripMenuItem1";
        this.kíchAppToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
        this.kíchAppToolStripMenuItem1.Text = "Kích App";
        this.kíchAppToolStripMenuItem1.Click += new System.EventHandler(kíchAppToolStripMenuItem1_Click);
        this.kíchAppIGToolStripMenuItem.Name = "kíchAppIGToolStripMenuItem";
        this.kíchAppIGToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.kíchAppIGToolStripMenuItem.Text = "Kích App IG";
        this.kíchAppIGToolStripMenuItem.Click += new System.EventHandler(kíchAppIGToolStripMenuItem_Click);
        this.kíchNútKhángToolStripMenuItem.Name = "kíchNútKhángToolStripMenuItem";
        this.kíchNútKhángToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.kíchNútKhángToolStripMenuItem.Text = "Kích nút kháng";
        this.kíchNútKhángToolStripMenuItem.Click += new System.EventHandler(kíchNútKhángToolStripMenuItem_Click);
        this.tạoWAToolStripMenuItem.Name = "tạoWAToolStripMenuItem";
        this.tạoWAToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.tạoWAToolStripMenuItem.Text = "Tạo WA";
        this.tạoWAToolStripMenuItem.Click += new System.EventHandler(tạoWAToolStripMenuItem_Click);
        this.tạoTKQCToolStripMenuItem.Name = "tạoTKQCToolStripMenuItem";
        this.tạoTKQCToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.tạoTKQCToolStripMenuItem.Text = "Tạo TKQC";
        this.tạoTKQCToolStripMenuItem.Click += new System.EventHandler(tạoTKQCToolStripMenuItem_Click);
        this.xóaWAToolStripMenuItem.Name = "xóaWAToolStripMenuItem";
        this.xóaWAToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.xóaWAToolStripMenuItem.Text = "Xóa WA";
        this.xóaWAToolStripMenuItem.Click += new System.EventHandler(xóaWAToolStripMenuItem_Click);
        this.checkLimitTKẨnToolStripMenuItem.Name = "checkLimitTKẨnToolStripMenuItem";
        this.checkLimitTKẨnToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.checkLimitTKẨnToolStripMenuItem.Text = "Check Limit TK ẩn";
        this.checkLimitTKẨnToolStripMenuItem.Click += new System.EventHandler(checkLimitTKẨnToolStripMenuItem_Click);
        this.mởBMChromeToolStripMenuItem.Name = "mởBMChromeToolStripMenuItem";
        this.mởBMChromeToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
        this.mởBMChromeToolStripMenuItem.Text = "Mở BM Chrome Quality";
        this.mởBMChromeToolStripMenuItem.Click += new System.EventHandler(mởBMChromeToolStripMenuItem_Click);
        this.tabPage3.Controls.Add(this.groupBox5);
        this.tabPage3.Controls.Add(this.dtgvTKQC);
        this.tabPage3.Controls.Add(this.tsTKQC);
        this.tabPage3.Location = new System.Drawing.Point(4, 25);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage3.Size = new System.Drawing.Size(1511, 536);
        this.tabPage3.TabIndex = 2;
        this.tabPage3.Text = "Cấu Hình TKQC";
        this.tabPage3.UseVisualStyleBackColor = true;
        this.groupBox5.Controls.Add(this.tabControl4);
        this.groupBox5.Location = new System.Drawing.Point(8, 6);
        this.groupBox5.Name = "groupBox5";
        this.groupBox5.Size = new System.Drawing.Size(1484, 229);
        this.groupBox5.TabIndex = 2;
        this.groupBox5.TabStop = false;
        this.groupBox5.Text = "Chức năng";
        this.tabControl4.Controls.Add(this.tabPage9);
        this.tabControl4.Controls.Add(this.tabPage13);
        this.tabControl4.Controls.Add(this.tabPage19);
        this.tabControl4.Controls.Add(this.tabPage20);
        this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabControl4.Location = new System.Drawing.Point(3, 19);
        this.tabControl4.Name = "tabControl4";
        this.tabControl4.SelectedIndex = 0;
        this.tabControl4.Size = new System.Drawing.Size(1478, 207);
        this.tabControl4.TabIndex = 0;
        this.tabPage9.Controls.Add(this.groupBox9);
        this.tabPage9.Controls.Add(this.groupBox7);
        this.tabPage9.Controls.Add(this.groupBox6);
        this.tabPage9.Location = new System.Drawing.Point(4, 25);
        this.tabPage9.Name = "tabPage9";
        this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage9.Size = new System.Drawing.Size(1470, 178);
        this.tabPage9.TabIndex = 0;
        this.tabPage9.Text = "BM Setting";
        this.tabPage9.UseVisualStyleBackColor = true;
        this.groupBox9.Controls.Add(this.btnShareTKQC);
        this.groupBox9.Controls.Add(this.tbLstUidVia);
        this.groupBox9.Location = new System.Drawing.Point(589, 6);
        this.groupBox9.Name = "groupBox9";
        this.groupBox9.Size = new System.Drawing.Size(242, 165);
        this.groupBox9.TabIndex = 6;
        this.groupBox9.TabStop = false;
        this.groupBox9.Text = "Share sang Via đã KB";
        this.btnShareTKQC.Location = new System.Drawing.Point(0, 99);
        this.btnShareTKQC.Name = "btnShareTKQC";
        this.btnShareTKQC.Size = new System.Drawing.Size(65, 25);
        this.btnShareTKQC.TabIndex = 6;
        this.btnShareTKQC.Text = "Share";
        this.btnShareTKQC.UseVisualStyleBackColor = true;
        this.btnShareTKQC.Click += new System.EventHandler(btnShareTKQC_Click);
        this.tbLstUidVia.Location = new System.Drawing.Point(6, 22);
        this.tbLstUidVia.Multiline = true;
        this.tbLstUidVia.Name = "tbLstUidVia";
        this.tbLstUidVia.Size = new System.Drawing.Size(230, 71);
        this.tbLstUidVia.TabIndex = 0;
        this.groupBox7.Controls.Add(this.btnBuildBM);
        this.groupBox7.Controls.Add(this.tbBMID);
        this.groupBox7.Controls.Add(this.label18);
        this.groupBox7.Location = new System.Drawing.Point(282, 6);
        this.groupBox7.Name = "groupBox7";
        this.groupBox7.Size = new System.Drawing.Size(284, 165);
        this.groupBox7.TabIndex = 5;
        this.groupBox7.TabStop = false;
        this.groupBox7.Text = "Nhét TKQC Vào BM";
        this.btnBuildBM.Location = new System.Drawing.Point(7, 67);
        this.btnBuildBM.Name = "btnBuildBM";
        this.btnBuildBM.Size = new System.Drawing.Size(85, 26);
        this.btnBuildBM.TabIndex = 2;
        this.btnBuildBM.Text = "Start";
        this.btnBuildBM.UseVisualStyleBackColor = true;
        this.btnBuildBM.Click += new System.EventHandler(btnBuildBM_Click);
        this.tbBMID.Location = new System.Drawing.Point(58, 25);
        this.tbBMID.Name = "tbBMID";
        this.tbBMID.Size = new System.Drawing.Size(210, 23);
        this.tbBMID.TabIndex = 0;
        this.label18.AutoSize = true;
        this.label18.Location = new System.Drawing.Point(7, 28);
        this.label18.Name = "label18";
        this.label18.Size = new System.Drawing.Size(45, 16);
        this.label18.TabIndex = 1;
        this.label18.Text = "BM ID:";
        this.groupBox6.Controls.Add(this.btnRemovePartner);
        this.groupBox6.Controls.Add(this.btnAssignPartner);
        this.groupBox6.Controls.Add(this.tbPartnerId);
        this.groupBox6.Controls.Add(this.label16);
        this.groupBox6.Controls.Add(this.label15);
        this.groupBox6.Controls.Add(this.cbbPermitTask);
        this.groupBox6.Location = new System.Drawing.Point(6, 6);
        this.groupBox6.Name = "groupBox6";
        this.groupBox6.Size = new System.Drawing.Size(260, 165);
        this.groupBox6.TabIndex = 4;
        this.groupBox6.TabStop = false;
        this.groupBox6.Text = "Share đối tác";
        this.btnRemovePartner.Location = new System.Drawing.Point(178, 129);
        this.btnRemovePartner.Name = "btnRemovePartner";
        this.btnRemovePartner.Size = new System.Drawing.Size(65, 25);
        this.btnRemovePartner.TabIndex = 5;
        this.btnRemovePartner.Text = "Xóa";
        this.btnRemovePartner.UseVisualStyleBackColor = true;
        this.btnRemovePartner.Click += new System.EventHandler(btnRemovePartner_Click);
        this.btnAssignPartner.Location = new System.Drawing.Point(79, 129);
        this.btnAssignPartner.Name = "btnAssignPartner";
        this.btnAssignPartner.Size = new System.Drawing.Size(65, 25);
        this.btnAssignPartner.TabIndex = 4;
        this.btnAssignPartner.Text = "Share";
        this.btnAssignPartner.UseVisualStyleBackColor = true;
        this.btnAssignPartner.Click += new System.EventHandler(btnAssignPartner_Click);
        this.tbPartnerId.Location = new System.Drawing.Point(79, 22);
        this.tbPartnerId.Multiline = true;
        this.tbPartnerId.Name = "tbPartnerId";
        this.tbPartnerId.Size = new System.Drawing.Size(164, 71);
        this.tbPartnerId.TabIndex = 0;
        this.label16.AutoSize = true;
        this.label16.Location = new System.Drawing.Point(6, 133);
        this.label16.Name = "label16";
        this.label16.Size = new System.Drawing.Size(48, 16);
        this.label16.TabIndex = 3;
        this.label16.Text = "Quyền:";
        this.label15.AutoSize = true;
        this.label15.Location = new System.Drawing.Point(6, 25);
        this.label15.Name = "label15";
        this.label15.Size = new System.Drawing.Size(67, 16);
        this.label15.TabIndex = 1;
        this.label15.Text = "Id Đối tác:";
        this.cbbPermitTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbPermitTask.FormattingEnabled = true;
        this.cbbPermitTask.Location = new System.Drawing.Point(79, 99);
        this.cbbPermitTask.Name = "cbbPermitTask";
        this.cbbPermitTask.Size = new System.Drawing.Size(164, 24);
        this.cbbPermitTask.TabIndex = 2;
        this.tabPage13.Controls.Add(this.groupBox8);
        this.tabPage13.Location = new System.Drawing.Point(4, 25);
        this.tabPage13.Name = "tabPage13";
        this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage13.Size = new System.Drawing.Size(1470, 178);
        this.tabPage13.TabIndex = 2;
        this.tabPage13.Text = "Campaign";
        this.tabPage13.UseVisualStyleBackColor = true;
        this.groupBox8.Controls.Add(this.btnRemoveGHCT);
        this.groupBox8.Controls.Add(this.btnSetGHCT);
        this.groupBox8.Controls.Add(this.tbGHCT);
        this.groupBox8.Location = new System.Drawing.Point(6, 6);
        this.groupBox8.Name = "groupBox8";
        this.groupBox8.Size = new System.Drawing.Size(274, 52);
        this.groupBox8.TabIndex = 0;
        this.groupBox8.TabStop = false;
        this.groupBox8.Text = "Set GHCT";
        this.btnRemoveGHCT.Location = new System.Drawing.Point(193, 23);
        this.btnRemoveGHCT.Name = "btnRemoveGHCT";
        this.btnRemoveGHCT.Size = new System.Drawing.Size(75, 23);
        this.btnRemoveGHCT.TabIndex = 2;
        this.btnRemoveGHCT.Text = "Gỡ";
        this.btnRemoveGHCT.UseVisualStyleBackColor = true;
        this.btnRemoveGHCT.Click += new System.EventHandler(btnRemoveGHCT_Click);
        this.btnSetGHCT.Location = new System.Drawing.Point(112, 22);
        this.btnSetGHCT.Name = "btnSetGHCT";
        this.btnSetGHCT.Size = new System.Drawing.Size(75, 23);
        this.btnSetGHCT.TabIndex = 1;
        this.btnSetGHCT.Text = "Set";
        this.btnSetGHCT.UseVisualStyleBackColor = true;
        this.btnSetGHCT.Click += new System.EventHandler(btnSetGHCT_Click);
        this.tbGHCT.Location = new System.Drawing.Point(6, 22);
        this.tbGHCT.Name = "tbGHCT";
        this.tbGHCT.Size = new System.Drawing.Size(100, 23);
        this.tbGHCT.TabIndex = 0;
        this.tbGHCT.Text = "0.01";
        this.tabPage19.Controls.Add(this.btnChangeInforAdAccount);
        this.tabPage19.Controls.Add(this.tbNameAdAccount);
        this.tabPage19.Controls.Add(this.label28);
        this.tabPage19.Controls.Add(this.tbRegionAdAccount);
        this.tabPage19.Controls.Add(this.label29);
        this.tabPage19.Controls.Add(this.cbbCurrencyAdAccount);
        this.tabPage19.Controls.Add(this.cbbTimeZoneAdAccount);
        this.tabPage19.Controls.Add(this.label30);
        this.tabPage19.Controls.Add(this.label31);
        this.tabPage19.Location = new System.Drawing.Point(4, 25);
        this.tabPage19.Name = "tabPage19";
        this.tabPage19.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage19.Size = new System.Drawing.Size(1470, 178);
        this.tabPage19.TabIndex = 3;
        this.tabPage19.Text = "Đổi thông tin";
        this.tabPage19.UseVisualStyleBackColor = true;
        this.btnChangeInforAdAccount.Location = new System.Drawing.Point(13, 107);
        this.btnChangeInforAdAccount.Name = "btnChangeInforAdAccount";
        this.btnChangeInforAdAccount.Size = new System.Drawing.Size(87, 27);
        this.btnChangeInforAdAccount.TabIndex = 26;
        this.btnChangeInforAdAccount.Text = "Change";
        this.btnChangeInforAdAccount.UseVisualStyleBackColor = true;
        this.btnChangeInforAdAccount.Click += new System.EventHandler(btnChangeInforAdAccount_Click);
        this.tbNameAdAccount.Location = new System.Drawing.Point(264, 69);
        this.tbNameAdAccount.Name = "tbNameAdAccount";
        this.tbNameAdAccount.Size = new System.Drawing.Size(112, 23);
        this.tbNameAdAccount.TabIndex = 24;
        this.tbNameAdAccount.Text = "ADS";
        this.label28.AutoSize = true;
        this.label28.Location = new System.Drawing.Point(224, 72);
        this.label28.Name = "label28";
        this.label28.Size = new System.Drawing.Size(34, 16);
        this.label28.TabIndex = 25;
        this.label28.Text = "Tên:";
        this.tbRegionAdAccount.Location = new System.Drawing.Point(78, 69);
        this.tbRegionAdAccount.Name = "tbRegionAdAccount";
        this.tbRegionAdAccount.Size = new System.Drawing.Size(91, 23);
        this.tbRegionAdAccount.TabIndex = 18;
        this.tbRegionAdAccount.Text = "CN";
        this.label29.AutoSize = true;
        this.label29.Location = new System.Drawing.Point(10, 72);
        this.label29.Name = "label29";
        this.label29.Size = new System.Drawing.Size(62, 16);
        this.label29.TabIndex = 23;
        this.label29.Text = "Quốc gia:";
        this.cbbCurrencyAdAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbCurrencyAdAccount.FormattingEnabled = true;
        this.cbbCurrencyAdAccount.Items.AddRange(new object[2] { "Tạo TKQC", "Tạo TKQC Share đối tác" });
        this.cbbCurrencyAdAccount.Location = new System.Drawing.Point(78, 37);
        this.cbbCurrencyAdAccount.Name = "cbbCurrencyAdAccount";
        this.cbbCurrencyAdAccount.Size = new System.Drawing.Size(298, 24);
        this.cbbCurrencyAdAccount.TabIndex = 21;
        this.cbbTimeZoneAdAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbTimeZoneAdAccount.FormattingEnabled = true;
        this.cbbTimeZoneAdAccount.Items.AddRange(new object[2] { "Tạo TKQC", "Tạo TKQC Share đối tác" });
        this.cbbTimeZoneAdAccount.Location = new System.Drawing.Point(78, 6);
        this.cbbTimeZoneAdAccount.Name = "cbbTimeZoneAdAccount";
        this.cbbTimeZoneAdAccount.Size = new System.Drawing.Size(298, 24);
        this.cbbTimeZoneAdAccount.TabIndex = 19;
        this.label30.AutoSize = true;
        this.label30.Location = new System.Drawing.Point(10, 40);
        this.label30.Name = "label30";
        this.label30.Size = new System.Drawing.Size(52, 16);
        this.label30.TabIndex = 22;
        this.label30.Text = "Tiền tệ:";
        this.label31.AutoSize = true;
        this.label31.Location = new System.Drawing.Point(10, 9);
        this.label31.Name = "label31";
        this.label31.Size = new System.Drawing.Size(53, 16);
        this.label31.TabIndex = 20;
        this.label31.Text = "Múi giờ:";
        this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.tabPage20.Controls.Add(this.btnAddPermission);
        this.tabPage20.Controls.Add(this.btnLoadBusinessUserName);
        this.tabPage20.Controls.Add(this.cbbBusinessUserName);
        this.tabPage20.Controls.Add(this.label32);
        this.tabPage20.Controls.Add(this.tbBusinessId);
        this.tabPage20.Location = new System.Drawing.Point(4, 25);
        this.tabPage20.Name = "tabPage20";
        this.tabPage20.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage20.Size = new System.Drawing.Size(1470, 178);
        this.tabPage20.TabIndex = 4;
        this.tabPage20.Text = "Thêm quyền TK cho USER";
        this.tabPage20.UseVisualStyleBackColor = true;
        this.btnAddPermission.Location = new System.Drawing.Point(451, 17);
        this.btnAddPermission.Name = "btnAddPermission";
        this.btnAddPermission.Size = new System.Drawing.Size(75, 23);
        this.btnAddPermission.TabIndex = 4;
        this.btnAddPermission.Text = "Thêm";
        this.btnAddPermission.UseVisualStyleBackColor = true;
        this.btnAddPermission.Click += new System.EventHandler(btnAddPermission_Click);
        this.btnLoadBusinessUserName.Location = new System.Drawing.Point(370, 17);
        this.btnLoadBusinessUserName.Name = "btnLoadBusinessUserName";
        this.btnLoadBusinessUserName.Size = new System.Drawing.Size(75, 23);
        this.btnLoadBusinessUserName.TabIndex = 3;
        this.btnLoadBusinessUserName.Text = "Load";
        this.btnLoadBusinessUserName.UseVisualStyleBackColor = true;
        this.btnLoadBusinessUserName.Click += new System.EventHandler(btnLoadBusinessUserName_Click);
        this.cbbBusinessUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbBusinessUserName.FormattingEnabled = true;
        this.cbbBusinessUserName.Location = new System.Drawing.Point(210, 17);
        this.cbbBusinessUserName.Name = "cbbBusinessUserName";
        this.cbbBusinessUserName.Size = new System.Drawing.Size(154, 24);
        this.cbbBusinessUserName.TabIndex = 2;
        this.label32.AutoSize = true;
        this.label32.Location = new System.Drawing.Point(6, 20);
        this.label32.Name = "label32";
        this.label32.Size = new System.Drawing.Size(45, 16);
        this.label32.TabIndex = 1;
        this.label32.Text = "BM ID:";
        this.tbBusinessId.Location = new System.Drawing.Point(57, 17);
        this.tbBusinessId.Name = "tbBusinessId";
        this.tbBusinessId.Size = new System.Drawing.Size(147, 23);
        this.tbBusinessId.TabIndex = 0;
        this.dtgvTKQC.AllowUserToAddRows = false;
        this.dtgvTKQC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.dtgvTKQC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dtgvTKQC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dtgvTKQC.Columns.AddRange(this.cSttTKQC, this.cIDTKQC, this.cUidViaTKQC, this.cNameTKQC, this.cAccountSpent, this.cStatusTKQC, this.cCurrencyTKQC, this.cLimitTKQC, this.cThreshold, this.cBalanceTKQC, this.cSpendCap, this.cPaymentTKQC, this.cTimeZoneTKQC, this.cBusiness, this.cCreateTimeTKQC, this.cOwnerTKQC, this.cRegionTKQC, this.cUserCountTKQC, this.cPartnerCount, this.cCampaignTKQC, this.cBussinessId, this.cProcessTKQC);
        this.dtgvTKQC.ContextMenuStrip = this.ctmnsTKQC;
        this.dtgvTKQC.Location = new System.Drawing.Point(8, 241);
        this.dtgvTKQC.Name = "dtgvTKQC";
        this.dtgvTKQC.RowHeadersVisible = false;
        this.dtgvTKQC.Size = new System.Drawing.Size(1495, 267);
        this.dtgvTKQC.TabIndex = 1;
        this.dtgvTKQC.KeyDown += new System.Windows.Forms.KeyEventHandler(dtgvTKQC_KeyDown);
        this.cSttTKQC.FillWeight = 30f;
        this.cSttTKQC.HeaderText = "Stt";
        this.cSttTKQC.MinimumWidth = 10;
        this.cSttTKQC.Name = "cSttTKQC";
        this.cIDTKQC.FillWeight = 45f;
        this.cIDTKQC.HeaderText = "ID TKQC";
        this.cIDTKQC.Name = "cIDTKQC";
        this.cUidViaTKQC.FillWeight = 45f;
        this.cUidViaTKQC.HeaderText = "Uid";
        this.cUidViaTKQC.Name = "cUidViaTKQC";
        this.cNameTKQC.FillWeight = 45f;
        this.cNameTKQC.HeaderText = "Tên";
        this.cNameTKQC.Name = "cNameTKQC";
        this.cAccountSpent.FillWeight = 30f;
        this.cAccountSpent.HeaderText = "Tổng chi";
        this.cAccountSpent.Name = "cAccountSpent";
        this.cStatusTKQC.FillWeight = 40f;
        this.cStatusTKQC.HeaderText = "Tình Trạng";
        this.cStatusTKQC.Name = "cStatusTKQC";
        this.cCurrencyTKQC.FillWeight = 36f;
        this.cCurrencyTKQC.HeaderText = "Tiền tệ";
        this.cCurrencyTKQC.Name = "cCurrencyTKQC";
        this.cLimitTKQC.FillWeight = 36f;
        this.cLimitTKQC.HeaderText = "Limit";
        this.cLimitTKQC.Name = "cLimitTKQC";
        this.cThreshold.FillWeight = 36f;
        this.cThreshold.HeaderText = "Ngưỡng";
        this.cThreshold.Name = "cThreshold";
        this.cBalanceTKQC.FillWeight = 36f;
        this.cBalanceTKQC.HeaderText = "Số dư";
        this.cBalanceTKQC.Name = "cBalanceTKQC";
        this.cSpendCap.FillWeight = 36f;
        this.cSpendCap.HeaderText = "GHCT";
        this.cSpendCap.Name = "cSpendCap";
        this.cPaymentTKQC.FillWeight = 60f;
        this.cPaymentTKQC.HeaderText = "PTTT";
        this.cPaymentTKQC.Name = "cPaymentTKQC";
        this.cTimeZoneTKQC.FillWeight = 75f;
        this.cTimeZoneTKQC.HeaderText = "Múi giờ";
        this.cTimeZoneTKQC.Name = "cTimeZoneTKQC";
        this.cBusiness.FillWeight = 45f;
        this.cBusiness.HeaderText = "Loại TK";
        this.cBusiness.Name = "cBusiness";
        this.cCreateTimeTKQC.FillWeight = 45f;
        this.cCreateTimeTKQC.HeaderText = "Ngày tạo";
        this.cCreateTimeTKQC.Name = "cCreateTimeTKQC";
        this.cOwnerTKQC.HeaderText = "ID Gốc";
        this.cOwnerTKQC.Name = "cOwnerTKQC";
        this.cRegionTKQC.FillWeight = 30f;
        this.cRegionTKQC.HeaderText = "QG";
        this.cRegionTKQC.Name = "cRegionTKQC";
        this.cUserCountTKQC.FillWeight = 40f;
        this.cUserCountTKQC.HeaderText = "QTV";
        this.cUserCountTKQC.Name = "cUserCountTKQC";
        this.cPartnerCount.FillWeight = 40f;
        this.cPartnerCount.HeaderText = "Đối tác";
        this.cPartnerCount.Name = "cPartnerCount";
        this.cCampaignTKQC.FillWeight = 40f;
        this.cCampaignTKQC.HeaderText = "Campaign";
        this.cCampaignTKQC.Name = "cCampaignTKQC";
        this.cBussinessId.FillWeight = 40f;
        this.cBussinessId.HeaderText = "ID BM";
        this.cBussinessId.Name = "cBussinessId";
        this.cProcessTKQC.FillWeight = 125f;
        this.cProcessTKQC.HeaderText = "Process";
        this.cProcessTKQC.Name = "cProcessTKQC";
        this.ctmnsTKQC.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.ctmnsTKQC.Items.AddRange(new System.Windows.Forms.ToolStripItem[21]
        {
            this.toolStripMenuItem1, this.tsmPasteBusinessId, this.toolStripMenuItem2, this.checkPTTTToolStripMenuItem, this.createRuleToolStripMenuItem, this.thoátTKQCToolStripMenuItem, this.thêmVàoDòng2BMToolStripMenuItem, this.xóaQTVToolStripMenuItem, this.xóaQTVIGToolStripMenuItem, this.thoátTKQCToolStripMenuItem1,
            this.đóngTKQCToolStripMenuItem, this.mởTKQCToolStripMenuItem, this.gánQuyềnAddThẻToolStripMenuItem, this.mởChromePEToolStripMenuItem, this.checkCampaignToolStripMenuItem, this.checkBillToolStripMenuItem, this.payToolStripMenuItem, this.cHECKTKQCDIEToolStripMenuItem, this.nhétTKQCToolStripMenuItem, this.deleteCreditCardToolStripMenuItem,
            this.deleteCampToolStripMenuItem
        });
        this.ctmnsTKQC.Name = "ctmnsBM";
        this.ctmnsTKQC.Size = new System.Drawing.Size(189, 422);
        this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        this.toolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
        this.toolStripMenuItem1.Text = "Dán ID";
        this.toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
        this.tsmPasteBusinessId.Name = "tsmPasteBusinessId";
        this.tsmPasteBusinessId.Size = new System.Drawing.Size(188, 22);
        this.tsmPasteBusinessId.Text = "Dán BM ID";
        this.tsmPasteBusinessId.Click += new System.EventHandler(tsmPasteBusinessId_Click);
        this.toolStripMenuItem2.Name = "toolStripMenuItem2";
        this.toolStripMenuItem2.Size = new System.Drawing.Size(188, 22);
        this.toolStripMenuItem2.Text = "Check TKQC";
        this.toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
        this.checkPTTTToolStripMenuItem.Name = "checkPTTTToolStripMenuItem";
        this.checkPTTTToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.checkPTTTToolStripMenuItem.Text = "Check PTTT";
        this.checkPTTTToolStripMenuItem.Click += new System.EventHandler(checkPTTTToolStripMenuItem_Click);
        this.createRuleToolStripMenuItem.Name = "createRuleToolStripMenuItem";
        this.createRuleToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.createRuleToolStripMenuItem.Text = "Tạo quy tắc";
        this.createRuleToolStripMenuItem.Click += new System.EventHandler(createRuleToolStripMenuItem_Click);
        this.deleteCreditCardToolStripMenuItem.Name = "deleteCreditCardToolStripMenuItem";
        this.deleteCreditCardToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.deleteCreditCardToolStripMenuItem.Text = "Xóa thẻ";
        this.deleteCreditCardToolStripMenuItem.Click += new System.EventHandler(deleteCreditCardToolStripMenuItem_Click);
        this.deleteCampToolStripMenuItem.Name = "deleteCampToolStripMenuItem";
        this.deleteCampToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.deleteCampToolStripMenuItem.Text = "Xóa Camp";
        this.deleteCampToolStripMenuItem.Click += new System.EventHandler(deleteCampToolStripMenuItem_Click);
        this.thoátTKQCToolStripMenuItem.Name = "thoátTKQCToolStripMenuItem";
        this.thoátTKQCToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.thoátTKQCToolStripMenuItem.Text = "Xóa TKQC Khỏi BM";
        this.thoátTKQCToolStripMenuItem.Click += new System.EventHandler(thoátTKQCToolStripMenuItem_Click);
        this.thêmVàoDòng2BMToolStripMenuItem.Name = "thêmVàoDòng2BMToolStripMenuItem";
        this.thêmVàoDòng2BMToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.thêmVàoDòng2BMToolStripMenuItem.Text = "Thêm vào dòng 2 BM";
        this.thêmVàoDòng2BMToolStripMenuItem.Click += new System.EventHandler(thêmVàoDòng2BMToolStripMenuItem_Click);
        this.xóaQTVToolStripMenuItem.Name = "xóaQTVToolStripMenuItem";
        this.xóaQTVToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.xóaQTVToolStripMenuItem.Text = "Xóa QTV";
        this.xóaQTVToolStripMenuItem.Click += new System.EventHandler(xóaQTVToolStripMenuItem_Click);
        this.xóaQTVIGToolStripMenuItem.Name = "xóaQTVIGToolStripMenuItem";
        this.xóaQTVIGToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.xóaQTVIGToolStripMenuItem.Text = "Xóa QTV IG";
        this.xóaQTVIGToolStripMenuItem.Click += new System.EventHandler(xóaQTVIGToolStripMenuItem_Click);
        this.thoátTKQCToolStripMenuItem1.Name = "thoátTKQCToolStripMenuItem1";
        this.thoátTKQCToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
        this.thoátTKQCToolStripMenuItem1.Text = "Thoát TKQC";
        this.thoátTKQCToolStripMenuItem1.Click += new System.EventHandler(thoátTKQCToolStripMenuItem1_Click);
        this.đóngTKQCToolStripMenuItem.Name = "đóngTKQCToolStripMenuItem";
        this.đóngTKQCToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.đóngTKQCToolStripMenuItem.Text = "Đóng TKQC";
        this.đóngTKQCToolStripMenuItem.Click += new System.EventHandler(đóngTKQCToolStripMenuItem_Click);
        this.mởTKQCToolStripMenuItem.Name = "mởTKQCToolStripMenuItem";
        this.mởTKQCToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.mởTKQCToolStripMenuItem.Text = "Mở TKQC";
        this.mởTKQCToolStripMenuItem.Click += new System.EventHandler(mởTKQCToolStripMenuItem_Click);
        this.gánQuyềnAddThẻToolStripMenuItem.Name = "gánQuyềnAddThẻToolStripMenuItem";
        this.gánQuyềnAddThẻToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.gánQuyềnAddThẻToolStripMenuItem.Text = "Gán quyền Add thẻ";
        this.gánQuyềnAddThẻToolStripMenuItem.Click += new System.EventHandler(gánQuyềnAddThẻToolStripMenuItem_Click);
        this.mởChromePEToolStripMenuItem.Name = "mởChromePEToolStripMenuItem";
        this.mởChromePEToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.mởChromePEToolStripMenuItem.Text = "Mở chrome PE";
        this.mởChromePEToolStripMenuItem.Click += new System.EventHandler(mởChromePEToolStripMenuItem_Click);
        this.checkCampaignToolStripMenuItem.Name = "checkCampaignToolStripMenuItem";
        this.checkCampaignToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.checkCampaignToolStripMenuItem.Text = "Check Campaign";
        this.checkCampaignToolStripMenuItem.Click += new System.EventHandler(checkCampaignToolStripMenuItem_Click);
        this.checkBillToolStripMenuItem.Name = "checkBillToolStripMenuItem";
        this.checkBillToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.checkBillToolStripMenuItem.Text = "Check Bill";
        this.checkBillToolStripMenuItem.Click += new System.EventHandler(checkBillToolStripMenuItem_Click);
        this.payToolStripMenuItem.Name = "payToolStripMenuItem";
        this.payToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.payToolStripMenuItem.Text = "Pay";
        this.payToolStripMenuItem.Click += new System.EventHandler(payToolStripMenuItem_Click);
        this.cHECKTKQCDIEToolStripMenuItem.Name = "cHECKTKQCDIEToolStripMenuItem";
        this.cHECKTKQCDIEToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.cHECKTKQCDIEToolStripMenuItem.Text = "CHECK TKQC DIE";
        this.cHECKTKQCDIEToolStripMenuItem.Click += new System.EventHandler(cHECKTKQCDIEToolStripMenuItem_Click);
        this.nhétTKQCToolStripMenuItem.Name = "nhétTKQCToolStripMenuItem";
        this.nhétTKQCToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
        this.nhétTKQCToolStripMenuItem.Text = "Nhét TKQC";
        this.nhétTKQCToolStripMenuItem.Click += new System.EventHandler(nhétTKQCToolStripMenuItem_Click);
        this.tsTKQC.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.tsTKQC.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.tsTKQC.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripLabel3, this.lblTotalCountTKQC });
        this.tsTKQC.Location = new System.Drawing.Point(3, 508);
        this.tsTKQC.Name = "tsTKQC";
        this.tsTKQC.Size = new System.Drawing.Size(1505, 25);
        this.tsTKQC.TabIndex = 0;
        this.tsTKQC.Text = "toolStrip1";
        this.toolStripLabel3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.toolStripLabel3.Name = "toolStripLabel3";
        this.toolStripLabel3.Size = new System.Drawing.Size(42, 22);
        this.toolStripLabel3.Text = "Tổng:";
        this.lblTotalCountTKQC.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.lblTotalCountTKQC.Name = "lblTotalCountTKQC";
        this.lblTotalCountTKQC.Size = new System.Drawing.Size(15, 22);
        this.lblTotalCountTKQC.Text = "0";
        this.tabPage6.Controls.Add(this.dtgvUser);
        this.tabPage6.Location = new System.Drawing.Point(4, 25);
        this.tabPage6.Name = "tabPage6";
        this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage6.Size = new System.Drawing.Size(1511, 536);
        this.tabPage6.TabIndex = 3;
        this.tabPage6.Text = "User";
        this.tabPage6.UseVisualStyleBackColor = true;
        this.dtgvUser.AllowUserToAddRows = false;
        this.dtgvUser.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.dtgvUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dtgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dtgvUser.Columns.AddRange(this.cSttUser, this.cUidUser, this.cUserId, this.cBMIdUser, this.cMailUser, this.cNameUser, this.cRoleUser, this.cStatusUser);
        this.dtgvUser.ContextMenuStrip = this.ctmnsUser;
        this.dtgvUser.Location = new System.Drawing.Point(8, 6);
        this.dtgvUser.Name = "dtgvUser";
        this.dtgvUser.RowHeadersVisible = false;
        this.dtgvUser.Size = new System.Drawing.Size(1133, 499);
        this.dtgvUser.TabIndex = 0;
        this.dtgvUser.KeyDown += new System.Windows.Forms.KeyEventHandler(dtgvUser_KeyDown);
        this.cSttUser.HeaderText = "Stt";
        this.cSttUser.Name = "cSttUser";
        this.cUidUser.HeaderText = "Uid";
        this.cUidUser.Name = "cUidUser";
        this.cUserId.HeaderText = "UserId";
        this.cUserId.Name = "cUserId";
        this.cBMIdUser.HeaderText = "BM ID";
        this.cBMIdUser.Name = "cBMIdUser";
        this.cMailUser.HeaderText = "Mail";
        this.cMailUser.Name = "cMailUser";
        this.cNameUser.HeaderText = "Name";
        this.cNameUser.Name = "cNameUser";
        this.cRoleUser.HeaderText = "Role";
        this.cRoleUser.Name = "cRoleUser";
        this.cStatusUser.HeaderText = "Status";
        this.cStatusUser.Name = "cStatusUser";
        this.ctmnsUser.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.ctmnsUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.xóaQuảnTrịToolStripMenuItem, this.xóaQuảnTrịTrước7NgàyToolStripMenuItem, this.xóaQuảnTrịBằngIGToolStripMenuItem, this.xóaLờiMờiToolStripMenuItem, this.hạQuyềnToolStripMenuItem, this.xóaLờiMờiToolStripMenuItem1 });
        this.ctmnsUser.Name = "ctmnsUser";
        this.ctmnsUser.Size = new System.Drawing.Size(208, 136);
        this.xóaQuảnTrịToolStripMenuItem.Name = "xóaQuảnTrịToolStripMenuItem";
        this.xóaQuảnTrịToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
        this.xóaQuảnTrịToolStripMenuItem.Text = "Xóa quản trị";
        this.xóaQuảnTrịToolStripMenuItem.Click += new System.EventHandler(xóaQuảnTrịToolStripMenuItem_Click);
        this.xóaQuảnTrịTrước7NgàyToolStripMenuItem.Name = "xóaQuảnTrịTrước7NgàyToolStripMenuItem";
        this.xóaQuảnTrịTrước7NgàyToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
        this.xóaQuảnTrịTrước7NgàyToolStripMenuItem.Text = "Xóa quản trị trước 7 ngày";
        this.xóaQuảnTrịTrước7NgàyToolStripMenuItem.Click += new System.EventHandler(xóaQuảnTrịTrước7NgàyToolStripMenuItem_Click);
        this.xóaQuảnTrịBằngIGToolStripMenuItem.Name = "xóaQuảnTrịBằngIGToolStripMenuItem";
        this.xóaQuảnTrịBằngIGToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
        this.xóaQuảnTrịBằngIGToolStripMenuItem.Text = "Xóa quản trị bằng IG";
        this.xóaQuảnTrịBằngIGToolStripMenuItem.Click += new System.EventHandler(xóaQuảnTrịBằngIGToolStripMenuItem_Click);
        this.xóaLờiMờiToolStripMenuItem.Name = "xóaLờiMờiToolStripMenuItem";
        this.xóaLờiMờiToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
        this.xóaLờiMờiToolStripMenuItem.Text = "Xóa lời mời";
        this.xóaLờiMờiToolStripMenuItem.Visible = false;
        this.xóaLờiMờiToolStripMenuItem.Click += new System.EventHandler(xóaLờiMờiToolStripMenuItem_Click);
        this.hạQuyềnToolStripMenuItem.Name = "hạQuyềnToolStripMenuItem";
        this.hạQuyềnToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
        this.hạQuyềnToolStripMenuItem.Text = "Hạ quyền";
        this.hạQuyềnToolStripMenuItem.Click += new System.EventHandler(hạQuyềnToolStripMenuItem_Click);
        this.xóaLờiMờiToolStripMenuItem1.Name = "xóaLờiMờiToolStripMenuItem1";
        this.xóaLờiMờiToolStripMenuItem1.Size = new System.Drawing.Size(207, 22);
        this.xóaLờiMờiToolStripMenuItem1.Text = "Xóa lời mời";
        this.xóaLờiMờiToolStripMenuItem1.Click += new System.EventHandler(xóaLờiMờiToolStripMenuItem1_Click);
        this.tabPage15.Controls.Add(this.tabControl5);
        this.tabPage15.Controls.Add(this.dtgvHotmail);
        this.tabPage15.Controls.Add(this.groupBox10);
        this.tabPage15.Location = new System.Drawing.Point(4, 25);
        this.tabPage15.Name = "tabPage15";
        this.tabPage15.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage15.Size = new System.Drawing.Size(1511, 536);
        this.tabPage15.TabIndex = 4;
        this.tabPage15.Text = "Hotmail";
        this.tabPage15.UseVisualStyleBackColor = true;
        this.tabControl5.Controls.Add(this.tabPage16);
        this.tabControl5.Controls.Add(this.tabPage17);
        this.tabControl5.Location = new System.Drawing.Point(1096, 17);
        this.tabControl5.Name = "tabControl5";
        this.tabControl5.SelectedIndex = 0;
        this.tabControl5.Size = new System.Drawing.Size(407, 511);
        this.tabControl5.TabIndex = 2;
        this.tabPage16.Controls.Add(this.webBrowser1);
        this.tabPage16.Location = new System.Drawing.Point(4, 25);
        this.tabPage16.Name = "tabPage16";
        this.tabPage16.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage16.Size = new System.Drawing.Size(399, 482);
        this.tabPage16.TabIndex = 0;
        this.tabPage16.Text = "Html";
        this.tabPage16.UseVisualStyleBackColor = true;
        this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.webBrowser1.Location = new System.Drawing.Point(3, 3);
        this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
        this.webBrowser1.Name = "webBrowser1";
        this.webBrowser1.Size = new System.Drawing.Size(393, 476);
        this.webBrowser1.TabIndex = 0;
        this.tabPage17.Location = new System.Drawing.Point(4, 25);
        this.tabPage17.Name = "tabPage17";
        this.tabPage17.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage17.Size = new System.Drawing.Size(399, 482);
        this.tabPage17.TabIndex = 1;
        this.tabPage17.Text = "Text";
        this.tabPage17.UseVisualStyleBackColor = true;
        this.dtgvHotmail.AllowUserToAddRows = false;
        this.dtgvHotmail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dtgvHotmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dtgvHotmail.Columns.AddRange(this.cSttMessage, this.cSender, this.cReceiveTime, this.cSubject, this.cCode, this.cBody, this.cBusinessLink);
        this.dtgvHotmail.ContextMenuStrip = this.ctmnsHotmail;
        this.dtgvHotmail.Location = new System.Drawing.Point(269, 17);
        this.dtgvHotmail.MultiSelect = false;
        this.dtgvHotmail.Name = "dtgvHotmail";
        this.dtgvHotmail.RowHeadersVisible = false;
        this.dtgvHotmail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dtgvHotmail.Size = new System.Drawing.Size(821, 511);
        this.dtgvHotmail.TabIndex = 1;
        this.dtgvHotmail.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dtgvHotmail_CellClick);
        this.cSttMessage.FillWeight = 30f;
        this.cSttMessage.HeaderText = "Stt";
        this.cSttMessage.Name = "cSttMessage";
        this.cSender.HeaderText = "Người gửi";
        this.cSender.Name = "cSender";
        this.cReceiveTime.FillWeight = 75f;
        this.cReceiveTime.HeaderText = "Thời gian";
        this.cReceiveTime.Name = "cReceiveTime";
        this.cSubject.HeaderText = "Tiêu đề";
        this.cSubject.Name = "cSubject";
        this.cCode.HeaderText = "Code";
        this.cCode.Name = "cCode";
        this.cBody.HeaderText = "Body";
        this.cBody.Name = "cBody";
        this.cBody.Visible = false;
        this.cBusinessLink.HeaderText = "Business Link";
        this.cBusinessLink.Name = "cBusinessLink";
        this.cBusinessLink.Visible = false;
        this.ctmnsHotmail.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.copyLinkToolStripMenuItem });
        this.ctmnsHotmail.Name = "ctmnsHotmail";
        this.ctmnsHotmail.Size = new System.Drawing.Size(128, 26);
        this.copyLinkToolStripMenuItem.Name = "copyLinkToolStripMenuItem";
        this.copyLinkToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
        this.copyLinkToolStripMenuItem.Text = "Copy Link";
        this.groupBox10.Controls.Add(this.ckbGetLinkBM);
        this.groupBox10.Controls.Add(this.btnDeleteAll);
        this.groupBox10.Controls.Add(this.btnCheckMail);
        this.groupBox10.Controls.Add(this.cbbDomain);
        this.groupBox10.Controls.Add(this.label26);
        this.groupBox10.Controls.Add(this.label25);
        this.groupBox10.Controls.Add(this.tbUsername);
        this.groupBox10.Cursor = System.Windows.Forms.Cursors.Default;
        this.groupBox10.Location = new System.Drawing.Point(8, 6);
        this.groupBox10.Name = "groupBox10";
        this.groupBox10.Size = new System.Drawing.Size(255, 522);
        this.groupBox10.TabIndex = 0;
        this.groupBox10.TabStop = false;
        this.ckbGetLinkBM.AutoSize = true;
        this.ckbGetLinkBM.Location = new System.Drawing.Point(12, 152);
        this.ckbGetLinkBM.Name = "ckbGetLinkBM";
        this.ckbGetLinkBM.Size = new System.Drawing.Size(92, 20);
        this.ckbGetLinkBM.TabIndex = 6;
        this.ckbGetLinkBM.Text = "Get Link BM";
        this.ckbGetLinkBM.UseVisualStyleBackColor = true;
        this.btnDeleteAll.Location = new System.Drawing.Point(120, 182);
        this.btnDeleteAll.Name = "btnDeleteAll";
        this.btnDeleteAll.Size = new System.Drawing.Size(102, 31);
        this.btnDeleteAll.TabIndex = 5;
        this.btnDeleteAll.Text = "Xóa All";
        this.btnDeleteAll.UseVisualStyleBackColor = true;
        this.btnDeleteAll.Click += new System.EventHandler(btnDeleteAll_Click);
        this.btnCheckMail.Location = new System.Drawing.Point(12, 182);
        this.btnCheckMail.Name = "btnCheckMail";
        this.btnCheckMail.Size = new System.Drawing.Size(102, 31);
        this.btnCheckMail.TabIndex = 4;
        this.btnCheckMail.Text = "Check Mail";
        this.btnCheckMail.UseVisualStyleBackColor = true;
        this.btnCheckMail.Click += new System.EventHandler(btnCheckMail_Click);
        this.cbbDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbbDomain.FormattingEnabled = true;
        this.cbbDomain.Location = new System.Drawing.Point(9, 121);
        this.cbbDomain.Name = "cbbDomain";
        this.cbbDomain.Size = new System.Drawing.Size(213, 24);
        this.cbbDomain.TabIndex = 3;
        this.label26.AutoSize = true;
        this.label26.Location = new System.Drawing.Point(6, 101);
        this.label26.Name = "label26";
        this.label26.Size = new System.Drawing.Size(94, 16);
        this.label26.TabIndex = 2;
        this.label26.Text = "Select Domain:";
        this.label25.AutoSize = true;
        this.label25.Location = new System.Drawing.Point(6, 20);
        this.label25.Name = "label25";
        this.label25.Size = new System.Drawing.Size(121, 16);
        this.label25.TabIndex = 1;
        this.label25.Text = "Username or Email:";
        this.tbUsername.Location = new System.Drawing.Point(6, 39);
        this.tbUsername.Multiline = true;
        this.tbUsername.Name = "tbUsername";
        this.tbUsername.Size = new System.Drawing.Size(216, 59);
        this.tbUsername.TabIndex = 0;
        this.tabPage18.Controls.Add(this.dtgvIG);
        this.tabPage18.Location = new System.Drawing.Point(4, 25);
        this.tabPage18.Name = "tabPage18";
        this.tabPage18.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage18.Size = new System.Drawing.Size(1511, 536);
        this.tabPage18.TabIndex = 5;
        this.tabPage18.Text = "IG";
        this.tabPage18.UseVisualStyleBackColor = true;
        this.dtgvIG.AllowUserToAddRows = false;
        this.dtgvIG.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        this.dtgvIG.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dtgvIG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dtgvIG.Columns.AddRange(this.cSttIG, this.cUsernameIG, this.cPasswordIG, this.cKey2FAIG, this.cCookieIG, this.cProcessIG);
        this.dtgvIG.ContextMenuStrip = this.ctmsIG;
        this.dtgvIG.Location = new System.Drawing.Point(8, 18);
        this.dtgvIG.Name = "dtgvIG";
        this.dtgvIG.ReadOnly = true;
        this.dtgvIG.RowHeadersVisible = false;
        this.dtgvIG.Size = new System.Drawing.Size(1495, 484);
        this.dtgvIG.TabIndex = 0;
        this.cSttIG.FillWeight = 30f;
        this.cSttIG.HeaderText = "Stt";
        this.cSttIG.Name = "cSttIG";
        this.cSttIG.ReadOnly = true;
        this.cUsernameIG.HeaderText = "Username";
        this.cUsernameIG.Name = "cUsernameIG";
        this.cUsernameIG.ReadOnly = true;
        this.cPasswordIG.HeaderText = "Password";
        this.cPasswordIG.Name = "cPasswordIG";
        this.cPasswordIG.ReadOnly = true;
        this.cKey2FAIG.HeaderText = "Key 2FA";
        this.cKey2FAIG.Name = "cKey2FAIG";
        this.cKey2FAIG.ReadOnly = true;
        this.cCookieIG.HeaderText = "Cookie";
        this.cCookieIG.Name = "cCookieIG";
        this.cCookieIG.ReadOnly = true;
        this.cProcessIG.HeaderText = "Process";
        this.cProcessIG.Name = "cProcessIG";
        this.cProcessIG.ReadOnly = true;
        this.ctmsIG.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.dánIGToolStripMenuItem, this.checkIGToolStripMenuItem, this.checkKíchToolStripMenuItem });
        this.ctmsIG.Name = "ctmsIG";
        this.ctmsIG.Size = new System.Drawing.Size(134, 70);
        this.dánIGToolStripMenuItem.Name = "dánIGToolStripMenuItem";
        this.dánIGToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
        this.dánIGToolStripMenuItem.Text = "Dán IG";
        this.dánIGToolStripMenuItem.Click += new System.EventHandler(dánIGToolStripMenuItem_Click);
        this.checkIGToolStripMenuItem.Name = "checkIGToolStripMenuItem";
        this.checkIGToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
        this.checkIGToolStripMenuItem.Text = "Check IG";
        this.checkIGToolStripMenuItem.Click += new System.EventHandler(checkIGToolStripMenuItem_Click);
        this.checkKíchToolStripMenuItem.Name = "checkKíchToolStripMenuItem";
        this.checkKíchToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
        this.checkKíchToolStripMenuItem.Text = "Check Kích";
        this.checkKíchToolStripMenuItem.Click += new System.EventHandler(checkKíchToolStripMenuItem_Click);
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(1519, 565);
        base.Controls.Add(this.panel1);
        base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
        base.Name = "Form1";
        base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Form1";
        base.Load += new System.EventHandler(Form1_Load);
        this.panel1.ResumeLayout(false);
        this.tabControl1.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.tabPage1.PerformLayout();
        this.tabControl2.ResumeLayout(false);
        this.tabPage4.ResumeLayout(false);
        this.tabPage4.PerformLayout();
        this.groupBox1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.nudCountThreads).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.dtgvVia).EndInit();
        this.ctmnsVia.ResumeLayout(false);
        this.tabPage5.ResumeLayout(false);
        this.tabPage5.PerformLayout();
        this.tsVia.ResumeLayout(false);
        this.tsVia.PerformLayout();
        this.tabPage2.ResumeLayout(false);
        this.tabPage2.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.tabControl3.ResumeLayout(false);
        this.tabPage7.ResumeLayout(false);
        this.groupBox3.ResumeLayout(false);
        this.groupBox3.PerformLayout();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel1.PerformLayout();
        this.tabPage8.ResumeLayout(false);
        this.groupBox4.ResumeLayout(false);
        this.groupBox4.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.nudCountNumAdAccount).EndInit();
        this.tabPage11.ResumeLayout(false);
        this.tabPage11.PerformLayout();
        this.tabPage10.ResumeLayout(false);
        this.tabPage10.PerformLayout();
        this.tabPage12.ResumeLayout(false);
        this.tabPage12.PerformLayout();
        this.tabPage14.ResumeLayout(false);
        this.tabPage14.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.nudCountCreateWA).EndInit();
        this.tsBM.ResumeLayout(false);
        this.tsBM.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvBM).EndInit();
        this.ctmnsBM.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        this.tabPage3.PerformLayout();
        this.groupBox5.ResumeLayout(false);
        this.tabControl4.ResumeLayout(false);
        this.tabPage9.ResumeLayout(false);
        this.groupBox9.ResumeLayout(false);
        this.groupBox9.PerformLayout();
        this.groupBox7.ResumeLayout(false);
        this.groupBox7.PerformLayout();
        this.groupBox6.ResumeLayout(false);
        this.groupBox6.PerformLayout();
        this.tabPage13.ResumeLayout(false);
        this.groupBox8.ResumeLayout(false);
        this.groupBox8.PerformLayout();
        this.tabPage19.ResumeLayout(false);
        this.tabPage19.PerformLayout();
        this.tabPage20.ResumeLayout(false);
        this.tabPage20.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.dtgvTKQC).EndInit();
        this.ctmnsTKQC.ResumeLayout(false);
        this.tsTKQC.ResumeLayout(false);
        this.tsTKQC.PerformLayout();
        this.tabPage6.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.dtgvUser).EndInit();
        this.ctmnsUser.ResumeLayout(false);
        this.tabPage15.ResumeLayout(false);
        this.tabControl5.ResumeLayout(false);
        this.tabPage16.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.dtgvHotmail).EndInit();
        this.ctmnsHotmail.ResumeLayout(false);
        this.groupBox10.ResumeLayout(false);
        this.groupBox10.PerformLayout();
        this.tabPage18.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.dtgvIG).EndInit();
        this.ctmsIG.ResumeLayout(false);
        base.ResumeLayout(false);
    }
}
